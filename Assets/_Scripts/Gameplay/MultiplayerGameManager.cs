using Photon.Pun;
using UnityEngine;
using TMPro;

public class MultiplayerGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _disconnectedText;

    private string _roomName;

    private void Start()
    {
        _roomName = PhotonNetwork.CurrentRoom.Name;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        photonView.RPC(UPDATE_PLAYER_LEAVE_TEXT, RpcTarget.All);
    }

    public void RejoinRoom()
    {
        PhotonNetwork.RejoinRoom(_roomName);
        PhotonMessageInfo info = new();
        photonView.RPC(Spaceship.RPC_SPAWN, RpcTarget.AllViaServer, transform.position, transform.rotation, info);
    }

    public const string UPDATE_PLAYER_LEAVE_TEXT = "RPC_UdatePlayerLeaveText";
    [PunRPC]
    public void RPC_UdatePlayerLeaveText()
    {
        _disconnectedText.text = PhotonNetwork.LocalPlayer.NickName + " disconnected";
    }
}
