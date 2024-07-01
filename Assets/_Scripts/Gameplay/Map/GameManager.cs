using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPunCallbacks
{
    private const string PlayerPrefabName = "Prefabs\\Game\\Ship";

    // !!!!REMOVE AFTER TESTING!!!
    [SerializeField] private MPTester _tester;
    [SerializeField] private bool _isOffline;

    private void Start()
    {
        if(_isOffline)
            PhotonNetwork.OfflineMode = true;

        if (!PhotonNetwork.OfflineMode)
            InitPlayer();
    }

    private void InitPlayer()
    {
        GameObject ship = PhotonNetwork.Instantiate(PlayerPrefabName, Vector3.zero, Quaternion.identity);
        ship.GetComponent<PlayerController>().enabled = true;
        ship.GetComponent<PlayerInput>().enabled = true;

        // !!! REMOVE AFTER TESTING !!!
        _tester.PlayerShip = ship.GetComponent<Spaceship>(); 

    }


}
