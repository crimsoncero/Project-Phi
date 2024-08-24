using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MPTester : MonoBehaviourPunCallbacks
{
    public Spaceship PlayerShip { get { return GameManager.Instance.ClientSpaceship; } }

    private string _roomName;

    private void Start()
    {
        _roomName = PhotonNetwork.CurrentRoom.Name;   
    }
    public void SetWeapon(int weaponEnum)
    {
        PlayerShip.photonView.RPC(Spaceship.RPC_SET_SPECIAL, Photon.Pun.RpcTarget.All, (WeaponEnum)weaponEnum);
    }

    public void ChangeMasterClient()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Player candidateMC = PhotonNetwork.LocalPlayer.GetNext();

            bool success = PhotonNetwork.SetMasterClient(candidateMC);
            Debug.Log("set master client result " + success);
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Reconnect()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            PhotonNetwork.RejoinRoom(_roomName);
        }
        else if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        else if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby.");
        PhotonNetwork.RejoinRoom(_roomName);
    }
}
