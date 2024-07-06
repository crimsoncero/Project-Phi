using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public event Action OnGameStarted;



    private const string SpaceshipPrefabPath = "Photon Prefabs\\Spaceship Photon";

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    
    [Header("Components")]
    [SerializeField] private CinemachineCamera _followCamera;
    [SerializeField] private GameObject _spawnPointContainer;
    [field: SerializeField] public WeaponList WeaponList { get; set; }
    [field: SerializeField] public ShipConfigList ShipConfigList { get; set; }

    [Header("Settings", order = 0)]

    [Header("Offline and NPCs", order = 1)]
    [SerializeField] private bool _isOffline;
    [Min(0)][SerializeField] private int _npcCount;

    [Header("Gameplay", order = 1)]
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private float _spawnPointCD;
    
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
    }

    private void Start()
    {
        if (_isOffline)
        {
            PhotonNetwork.OfflineMode = true;
        }
        else
        {
            InitPlayer();
        }

        

    }


    /// <summary>
    /// Kickstarts the game at the same time for all players. Only the master client can use this method.
    /// </summary>
    private void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        OnGameStarted?.Invoke();

        foreach(var spaceship in SpaceshipList)
        {
            SpawnShip(spaceship);
        }
    }



    private void InitPlayer()
    {
        GameObject ship = PhotonNetwork.Instantiate(SpaceshipPrefabPath, Vector3.zero, Quaternion.identity);
        ClientSpaceship = ship.GetComponent<Spaceship>();
        
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

            UIManager.Instance.Init(); // Initialize UI after player's ship was added.
        }



        // Master client check if all players ships are ready to start the game:
        if (PhotonNetwork.IsMasterClient && SpaceshipList.Count == PhotonNetwork.CurrentRoom.PlayerCount + _npcCount)
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

    private void SpawnShip(Spaceship ship)
    {
        Transform transform = GetSpawnPoint();
        ship.photonView.RPC(Spaceship.RPC_SPAWN, RpcTarget.AllViaServer, transform.position, transform.rotation);
    }

    private Transform GetSpawnPoint()
    {
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

}
