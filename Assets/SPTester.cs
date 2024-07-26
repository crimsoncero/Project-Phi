using MoreMountains.Tools;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class SPTester : MonoBehaviourPunCallbacks
{

    private const string SpaceshipPrefabPath = "Photon Prefabs\\Spaceship Photon";
    private const string SynchronizerPrefabPath = "Photon Prefabs\\Synchronizer";

    [SerializeField] private Transform _trainingDummySpawn;
    [SerializeField] private MMProgressBar _heatBar;
    private Spaceship _playerShip;

    private Spaceship _dummyShip;
    
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
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
        PhotonNetwork.InstantiateRoomObject(SynchronizerPrefabPath, Vector3.zero, Quaternion.identity);

        GameObject ship = PhotonNetwork.Instantiate(SpaceshipPrefabPath, Vector3.zero, Quaternion.identity);
        ship.name = "PlayerShip";
        
        _playerShip = ship.GetComponent<Spaceship>();

        GameObject dummy = PhotonNetwork.InstantiateRoomObject(SpaceshipPrefabPath, _trainingDummySpawn.position, _trainingDummySpawn.rotation);
        dummy.name = "Training Dummy";
        _dummyShip = dummy.GetComponent<Spaceship>();
    }

    public void SetWeapon(int weaponEnum)
    {
        _playerShip.photonView.RPC(Spaceship.RPC_SET_SPECIAL, Photon.Pun.RpcTarget.All, (WeaponEnum)weaponEnum);
    }

    
}
