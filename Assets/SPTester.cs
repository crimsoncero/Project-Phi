using MoreMountains.Tools;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SPTester : MonoBehaviourPunCallbacks
{
    [SerializeField] private MMProgressBar _heatBar;
    [SerializeField] private GameObject _player;
    private PlayerController _playerController;
    private Spaceship _shipController;
    
    
    private void Start()
    {
        PhotonNetwork.OfflineMode = true;
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _shipController.OnHeatChanged -= UpdateHeatBar;
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
        _shipController.OnHeatChanged += UpdateHeatBar;
    }

    public void SetWeapon(int weaponEnum)
    {
        _shipController.photonView.RPC(Spaceship.RPC_SET_SPECIAL, Photon.Pun.RpcTarget.All, (WeaponEnum)weaponEnum);
    }

    public void UpdateHeatBar(float progress)
    {
        _heatBar.UpdateBar(_shipController.PrimaryHeat, 0, _shipController.PrimaryWeapon.MaxHeat);
    }
}
