using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class SPTester : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _player;
    
    private PlayerController _playerController;
    private void Start()
    {
        PhotonNetwork.OfflineMode = true;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.CreateRoom("SinglePlayer");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Init();
    }

    private void Init()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _playerController.enabled = true;
        _player.GetComponent<PlayerInput>().enabled = true;
    }
}
