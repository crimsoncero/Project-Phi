using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class Synchronizer : MonoBehaviourPun, IPunObservable
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
            Debug.Log("Updated Timer");
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
        Timer = MatchTimerGoal;


        while (true)
        {
            yield return new WaitForSeconds(1);

            if (MatchTimerGoal == 0)
                Timer += 1;
            else
                Timer -= 1;

            if (Timer == 0)
            {
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
}
