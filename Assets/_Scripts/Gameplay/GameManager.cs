using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public event Action OnGameStarted;

    private const string SpaceshipPrefabPath = "Photon Prefabs\\Spaceship Photon";
    private const string WeaponPickupPrefabPath = "Photon Prefabs\\Weapon Pickup";
    private const string SynchronizerPrefabPath = "Photon Prefabs\\Synchronizer";

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("Components")]
    [SerializeField] private CinemachineCamera _followCamera;
    [SerializeField] private GameObject _spawnPointContainer;
    [SerializeField] private GameObject _weaponSpawnersContainer;
    [field: SerializeField] public WeaponList WeaponList { get; set; }
    [field: SerializeField] public ShipConfigList ShipConfigList { get; set; }

    [Header("Settings", order = 0)]

    [Header("Offline and NPCs", order = 1)]
    [SerializeField] private bool _isOffline;
    [Min(0)][SerializeField] private int _npcCount;

    [Header("Gameplay", order = 1)]
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private float _spawnPointCD;
    [SerializeField] private float _weaponSpawnCD;
    [SerializeField] private int _startingWeaponCount;
    
    /// <summary>
    /// The Spaceship is controlled by this client player.
    /// </summary>
    public Spaceship ClientSpaceship { get; private set; } = null;
    /// <summary>
    /// A list of all registered spaceships in the game.
    /// </summary>
    public List<Spaceship> SpaceshipList { get; private set; } = new List<Spaceship>();
    
    /// <summary>
    /// A Dictionary of all the spawn points as keys, and whether they are ready to be used or not as their value.
    /// </summary>
    private Dictionary<Transform, bool> _spawnPoints;

    private Synchronizer _synchronizer;
    private PlayerScoreHandler _scoreHandler;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }

        // Init Spawn Point Dictionary:

        _spawnPoints = new Dictionary<Transform, bool>();
        foreach (Transform child in _spawnPointContainer.transform)
        {
            if (child == _spawnPointContainer.transform) return;
            _spawnPoints.Add(child, true);
        }

        Synchronizer.OnMatchFinished += EndGame;

    }

    private void Start()
    {
        if (_isOffline)
        {
            PhotonNetwork.OfflineMode = true;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(SynchronizerPrefabPath, Vector3.zero, Quaternion.identity);
            }

            InitPlayer();
        }
    }


    /// <summary>
    /// Kickstarts the game at the same time for all players. Only the master client can use this method.
    /// </summary>
    private void StartGame()
    {
        _scoreHandler = new(PhotonNetwork.CurrentRoom.Players.Values.ToList());
        
        if (PhotonNetwork.IsMasterClient)
        {
            OnGameStarted?.Invoke();

            foreach (var spaceship in SpaceshipList)
            {
                SpawnShip(spaceship);
            }

            _synchronizer.photonView.RPC(Synchronizer.RPC_START_MATCH, RpcTarget.All);
            SpawnWeapons();
        }
    }

    private void EndGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.PlayerTtl = 0;
            PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
        }

        Time.timeScale = 0;
        // DISPLAY SCOREBOARD FOR PLAYERS
        
    }

    private void InitPlayer()
    {
        GameObject ship = PhotonNetwork.Instantiate(SpaceshipPrefabPath, Vector3.zero, Quaternion.identity);
        ClientSpaceship = ship.GetComponent<Spaceship>();
    }

    public void RegisterSynchronizer(Synchronizer synchronizer)
    {
        _synchronizer = synchronizer;
    }

    public void RegisterSpaceship(Spaceship spaceship)
    {
        // Register ship and set its initial state.
        if (!SpaceshipList.Contains(spaceship))
        {
            SpaceshipList.Add(spaceship);
            spaceship.name = $"{spaceship.photonView.Owner.NickName}'s Ship";

            spaceship.SetConfig();

            // Deactivate ship until master client starts the game.
            spaceship.gameObject.SetActive(false);
        }

        // Actions to do after registering the client's ship.
        if (spaceship.photonView.CreatorActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            ClientSpaceship = spaceship;
            _followCamera.Follow = spaceship.transform;

            
        }

        if (SpaceshipList.Count == PhotonNetwork.CurrentRoom.PlayerCount + _npcCount)
            StartGame();
    }

    public Spaceship FindSpaceship(GameObject shipObject)
    {
        if (shipObject.IsUnityNull()) return null;
        int shipIndex = SpaceshipList.FindIndex((s) => s.gameObject == shipObject);
        if(shipIndex < 0 ) return null;

        return SpaceshipList[shipIndex];
    }
    public Spaceship FindSpaceship(Player player)
    {
        if(player == null) return null;
        int shipIndex = SpaceshipList.FindIndex((s) => s.photonView.Owner == player);
        if (shipIndex < 0) return null;

        return SpaceshipList[shipIndex];
    }

    public void WeaponPickedUp(WeaponPickup weapon, bool isInit = false)
    {
        if(!isInit)
            weapon.photonView.RPC(WeaponPickup.RPC_DEACTIVATE, RpcTarget.All);
        
        StartCoroutine(SpawnNewWeapon(weapon));
    }

    public void SpawnShip(Spaceship ship, bool isRespawn = false)
    {
        if (isRespawn)
            StartCoroutine(WaitForRespawn(ship));
        else
        {
            Transform transform = GetSpawnPoint();
            ship.photonView.RPC(Spaceship.RPC_SPAWN, RpcTarget.AllViaServer, transform.position, transform.rotation);
        }
    }
    private IEnumerator WaitForRespawn(Spaceship ship)
    {
        yield return new WaitForSeconds(_timeToSpawn);
        SpawnShip(ship, false);
    }

    private Transform GetSpawnPoint()
    {
        if (!PhotonNetwork.IsMasterClient)
            throw new Exception("Can't run this on non master client");

        // default spawn point if there no spawn points were made.
        Transform p = transform;
        p.position = Vector3.zero;
        p.rotation = Quaternion.identity;

        if (_spawnPoints == null || _spawnPoints.Count == 0) return p; 

        // Create array of all the spawn points that are currently ready.
        Transform[] readySpawns = _spawnPoints.Where((p) => p.Value).Select((p) => p.Key).ToArray();
        
        // If there are no available spawn points, reset all the spawn points and try again.
        if(readySpawns.Length == 0)
        {
            ResetSpawnPoints();
            return GetSpawnPoint();
        }


        int index = Random.Range(0, readySpawns.Length);

        StartCoroutine(SpawnPointCooldown(readySpawns[index]));

        return readySpawns[index];
    }

    private void ResetSpawnPoints()
    {
        if (_spawnPoints.Count == 0) return;

        foreach(var key in  _spawnPoints.Keys)
        {
            _spawnPoints[key] = true;
        }
    }

    private IEnumerator SpawnPointCooldown(Transform spawnPoint)
    {
        _spawnPoints[spawnPoint] = false;

        yield return new WaitForSeconds(_spawnPointCD);

        _spawnPoints[spawnPoint] = true;
    }
    private void SpawnWeapons()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_weaponSpawnersContainer == null) return;

        List<Transform> positions = new();

        foreach(Transform child in _weaponSpawnersContainer.transform)
        {
            if (child == _weaponSpawnersContainer.transform) return;
            positions.Add(child);
           
        }

        positions.MMShuffle();
        int spawnedWeapon = 0;
        foreach(var position in positions)
        {
            object[] data = new object[2];
            bool isReady = false;
            data[0] = isReady;
            if(spawnedWeapon < _startingWeaponCount)
            {
                spawnedWeapon++;
                WeaponEnum w = WeaponList.GetRandomWeaponEnum();
                isReady = true;

                data[0] = isReady;
                data[1] = w;
            }

            GameObject weapon = PhotonNetwork.InstantiateRoomObject(WeaponPickupPrefabPath, position.position, position.rotation, 0, data);
            weapon.transform.SetParent(_weaponSpawnersContainer.transform);

        }
    }
    private IEnumerator SpawnNewWeapon(WeaponPickup weapon)
    {
        yield return new WaitForSeconds(_weaponSpawnCD);

        WeaponEnum w = WeaponList.GetRandomWeaponEnum();

        weapon.photonView.RPC(WeaponPickup.RPC_ACTIVATE_WEAPON_PICKUP, RpcTarget.All, w);
    }


    public void IncreasePlayerScore(Player player, int amount)
    {
        _scoreHandler.IncreasePlayerScore(player, amount);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        _scoreHandler.UnlockPlayerScore(targetPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);



    }
}
