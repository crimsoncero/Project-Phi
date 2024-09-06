using Photon.Pun;
using UnityEngine;

public class PauseHandler : MonoBehaviourPunCallbacks
{


    private bool _pauseLeave = false;


    public void Resume()
    {
        UIManager.Instance.OnResumeFromPause();
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom(false);
        _pauseLeave = true;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (_pauseLeave)
            SceneLoader.LoadMainMenu();
    }

}
