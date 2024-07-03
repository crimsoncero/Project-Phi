using MoreMountains.Tools;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public class SPTester : MonoBehaviourPunCallbacks
{

    private const string SpaceshipPrefabPath = "Photon Prefabs\\Spaceship Photon";
    [SerializeField] private Transform _trainingDummySpawn;
    [SerializeField] private MMProgressBar _heatBar;
    private Spaceship _shipController;
    
    
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
        GameObject ship = PhotonNetwork.Instantiate(SpaceshipPrefabPath, Vector3.zero, Quaternion.identity);
        ship.name = "PlayerShip";
        ship.GetComponent<PlayerController>().enabled = true;
        ship.GetComponent<PlayerInput>().enabled = true;
        _shipController = ship.GetComponent<Spaceship>();


        GameObject dummy = PhotonNetwork.InstantiateRoomObject(SpaceshipPrefabPath, _trainingDummySpawn.position, _trainingDummySpawn.rotation);
        dummy.name = "Training Dummy";
    }

    public void SetWeapon(int weaponEnum)
    {
        _shipController.photonView.RPC(Spaceship.RPC_SET_SPECIAL, Photon.Pun.RpcTarget.All, (WeaponEnum)weaponEnum);
    }

    
}
