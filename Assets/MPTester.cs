using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MPTester : MonoBehaviourPun
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
        if(PhotonNetwork.InLobby)
            PhotonNetwork.RejoinRoom(_roomName);
    }
}
