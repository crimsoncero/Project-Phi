using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPunCallbacks
{
    public event Action OnGameStarted;



    private const string SpaceshipPrefabPath = "Photon Prefabs\\Spaceship Photon";

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    
    [Header("Components")]
    [SerializeField] private CinemachineCamera _followCamera;

    /// <summary>
    /// The Spaceship is controlled by this client player.
    /// </summary>
    public Spaceship ClientSpaceship { get; private set; } = null;

    /// <summary>
    /// A list of all registered spaceships in the game.
    /// </summary>
    public List<Spaceship> SpaceshipList { get; private set; } = new List<Spaceship>();

    [Header("Settings")]
    [SerializeField] private bool _isOffline;
    [Min(0)][SerializeField] private int _npcCount;
    [field: SerializeField] public WeaponList WeaponList { get; set; }
    [field: SerializeField] public ShipConfigList ShipConfigList { get; set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
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
            spaceship.photonView.RPC(Spaceship.RPC_SPAWN, RpcTarget.AllViaServer, Vector3.zero, Quaternion.identity);

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

}
