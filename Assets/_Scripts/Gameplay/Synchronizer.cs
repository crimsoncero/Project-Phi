using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Synchronizer : MonoBehaviourPunCallbacks, IPunObservable
{

    public static event Action<int> OnTimerUpdated;
	public static event Action OnMatchStarted;
	public static event Action<EndGamePlayerData[]> OnMatchFinished;


	private int _timer = 0;
	public int Timer
	{
		get { return _timer; }
		set 
		{
			_timer = value;
			OnTimerUpdated?.Invoke(value);
		}
	}

    public int MatchStartTime { get; private set; }
    public int MatchTimerGoal { get; private set; }
    public int MatchScoreGoal { get; private set; }
    public bool IsMatchActive { get; private set; }


    private void Awake()
    {

        RoomProperties props = new RoomProperties();

        if(PhotonNetwork.OfflineMode)
        {
            props.Time = 0;
            props.Score = 0;
        }
        else
        {
            props = new RoomProperties(PhotonNetwork.CurrentRoom.CustomProperties);
        }

        MatchTimerGoal = props.Time;
        MatchScoreGoal = props.Score;
        IsMatchActive = true;


        if (PhotonNetwork.IsMasterClient)
        {
            Timer = MatchTimerGoal;
        }
    }

    private void Start()
    {
        GameManager.Instance.RegisterSynchronizer(this);
    }


    private IEnumerator TimerTick()
	{
        if (!PhotonNetwork.IsMasterClient) yield break;

        while (true)
        {
            yield return new WaitForSeconds(1);

            if (MatchTimerGoal == 0)
                Timer += 1;
            else
                Timer -= 1;

            if (Timer == 0 && MatchTimerGoal > 0)
            {
                photonView.RPC(RPC_END_MATCH, RpcTarget.AllViaServer, CreateEndGameData());
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send Data
            stream.SendNext(Timer);
        }
        else
        {
            // Reciece Data
            Timer = (int)stream.ReceiveNext();
        }

    }

    public EndGamePlayerData[] CreateEndGameData()
    {
        Player[] players = PhotonNetwork.CurrentRoom.Players.Values.ToArray();

        EndGamePlayerData[] data = new EndGamePlayerData[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            data[i] = new EndGamePlayerData();
            data[i].ActorNumber = players[i].ActorNumber;
            data[i].ConfigID = players[i].GetShipConfigID();
            data[i].Score = players[i].GetPlayerKills();
        }

        return data;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);


        if(PhotonNetwork.IsMasterClient)
        {
            if(MatchScoreGoal > 0 && targetPlayer.GetPlayerKills() == MatchScoreGoal)
            {
                photonView.RPC(RPC_END_MATCH, RpcTarget.AllViaServer, CreateEndGameData());
            }
        }
    }




    public static string RPC_START_MATCH = "RPC_StartMatch";
    [PunRPC]
    public void RPC_StartMatch()
    {
        OnMatchStarted?.Invoke();

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerTick());
        }
    }

    public static string RPC_END_MATCH = "RPC_EndMatch";
    [PunRPC]
    public void RPC_EndMatch(EndGamePlayerData[] endGameData)
    {
        IsMatchActive = false;
        OnMatchFinished?.Invoke(endGameData);

        PlayerPrefsHandler.SetRoomId("");

        // Lock Room
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.PlayerTtl = 0;
            PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;

            StartCoroutine(WaitEmpty());
        }
        else
        {
            PhotonNetwork.LeaveRoom(false);
        }

    }

    private IEnumerator WaitEmpty()
    {
        while(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            yield return new WaitForSeconds(0.1f);
        }

        PhotonNetwork.LeaveRoom(false);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerTick());
        }
    }
}
