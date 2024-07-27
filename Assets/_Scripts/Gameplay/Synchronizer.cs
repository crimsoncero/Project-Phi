using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class Synchronizer : MonoBehaviourPunCallbacks, IPunObservable
{
    public static event Action<int> OnTimerUpdated;
	public static event Action OnMatchStarted;
	public static event Action OnMatchFinished;


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
    }


    private IEnumerator TimerTick()
	{
        if (!PhotonNetwork.IsMasterClient) yield break;
        
        Timer = MatchTimerGoal;

        while (true)
        {
            yield return new WaitForSeconds(1);

            if (MatchTimerGoal == 0)
                Timer += 1;
            else
                Timer -= 1;

            if (Timer == 0 && MatchTimerGoal > 0)
            {
                photonView.RPC(RPC_END_MATCH, RpcTarget.All);
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);


        if(PhotonNetwork.IsMasterClient)
        {
            if(MatchScoreGoal > 0 && targetPlayer.GetPlayerKills() == MatchScoreGoal)
            {
                photonView.RPC(RPC_END_MATCH, RpcTarget.All);
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
    public void RPC_EndMatch()
    {
        OnMatchFinished?.Invoke();
    }



}
