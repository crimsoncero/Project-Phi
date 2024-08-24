using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Synchronizer : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] TMP_Text _masterClientText;
    [SerializeField] TMP_Text _disconnectedText;

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
        GameManager.Instance.RegisterSynchronizer(this);

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
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(RPC_UPDATE_MC_NAME, RpcTarget.All, PhotonNetwork.MasterClient);
        }
    }

    public const string RPC_UPDATE_MC_NAME = "RPC_UpdateMCName";
    [PunRPC]
    public void RPC_UpdateMCName(Player newMasterClient)
    {
        _masterClientText.text = newMasterClient.ToString();
    }
    
    public const string RPC_UPDATE_DISCONNECT_NAME = "RPC_UpdateDisconnectName";
    private const string DISCONNECTED_MESSAGE = " disconnected";

    [PunRPC]
    public void RPC_UpdateDisconnectName(Player disconnectedPlayer)
    {
        _disconnectedText.text = disconnectedPlayer.ToString() + DISCONNECTED_MESSAGE;
    }

    private IEnumerator TimerTick(bool initTime = false)
	{
        if (!PhotonNetwork.IsMasterClient) yield break;
        
        if(initTime)
            Timer = MatchTimerGoal;

        while (true)
        {
            if (!PhotonNetwork.IsMasterClient) yield break;

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

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if(PhotonNetwork.IsMasterClient)
        {
            if (gameObject.activeSelf)
                StartCoroutine(TimerTick());
            photonView.RPC(RPC_UPDATE_MC_NAME, RpcTarget.All, PhotonNetwork.MasterClient);
        }
    }



    public static string RPC_START_MATCH = "RPC_StartMatch";
    [PunRPC]
    public void RPC_StartMatch()
    {
        OnMatchStarted?.Invoke();

        if (PhotonNetwork.IsMasterClient)
        {
            if (gameObject.activeSelf)
                StartCoroutine(TimerTick(true));
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
            if (gameObject.activeSelf)
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (!PhotonNetwork.IsMasterClient) return;

        if (IsMatchActive)
        {
            foreach(var ship in GameManager.Instance.SpaceshipList)
            {
                if(ship.isActiveAndEnabled)
                    ship.photonView.RPC(Spaceship.RPC_ACTIVATE, newPlayer);
            }
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        photonView.RPC(RPC_UPDATE_DISCONNECT_NAME, RpcTarget.All, otherPlayer);
    }
}
