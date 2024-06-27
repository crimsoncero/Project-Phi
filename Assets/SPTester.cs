using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SPTester : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Scrollbar _heatBar;
    private PlayerController _playerController;
    private Spaceship _shipController;
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
        _shipController = _player.GetComponent<Spaceship>();
        _playerController.enabled = true;
        _player.GetComponent<PlayerInput>().enabled = true;
    }

    private void Update()
    {
        float heatVal = Mathf.Lerp(0, 1, _shipController.PrimaryHeat / _shipController.PrimaryWeapon.MaximumHeat);
        _heatBar.size = heatVal;
    }


    public void SetWeapon(Weapon weapon)
    {
        _shipController.SetSpecialWeapon(weapon);
    }
}
