using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPunCallbacks
{
    private const string PlayerPrefabName = "Prefabs\\Game\\Ship";

    private void Start()
    {
       InitPlayer();
    }

    private void InitPlayer()
    {
        GameObject ship = PhotonNetwork.Instantiate(PlayerPrefabName, Vector3.zero, Quaternion.identity);
        ship.GetComponent<PlayerController>().enabled = true;
        ship.GetComponent<PlayerInput>().enabled = true;
    }


}
