using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer _weaponRenderer;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private Sprite _autoCannonSprite;
    [SerializeField] private Sprite _rocketPodSprite;
    [SerializeField] private Sprite _mineDispenserSprite;
    [SerializeField] private Sprite _doomLaserSprite;

    [field: SerializeField] public WeaponEnum WeaponEnum { get; private set; }
    public bool IsPickable { get; private set; } = false;
    public bool IsAvailable { get; private set; } = true;

    [SerializeField] private InputActionAsset _inputAsset;
    private InputActionMap _inputMap;
    private InputAction _onInteract;

    private void Start()
    {
        _inputMap = _inputAsset.FindActionMap("Player");
        _onInteract = _inputMap.FindAction("Interact");
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _onInteract.performed += OnInteract;

        _canvas.enabled = false;
    }

    private void OnDisable()
    {
        _onInteract.performed -= OnInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return; // Only consider collisions with player objects.
        if (other.gameObject != GameManager.Instance.ClientSpaceship.gameObject) return; // Only consider collisions with the clients spaceship.

        IsPickable = true;
        _canvas.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return; // Only consider collisions with player objects.
        if (other.gameObject != GameManager.Instance.ClientSpaceship.gameObject) return; // Only consider collisions with the clients spaceship.

        IsPickable = false;
        _canvas.enabled = false;

    }


    public void OnInteract(InputAction.CallbackContext obj)
    {
         
        if (!IsPickable) return;
        if (!IsAvailable) return;

        photonView.RPC(RPC_PICkUP_WEAPON, RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);

    }

    public static string RPC_PICkUP_WEAPON = "RPC_PickupWeapon";
    [PunRPC]
    private void RPC_PickupWeapon(Player player)
    {
        if (!PhotonNetwork.IsMasterClient) return; // Only works in master client.
        if (!IsAvailable) return; // Weapon already picked;

        IsAvailable = false;

        Spaceship ship = GameManager.Instance.SpaceshipList.Where((p) => p.photonView.Owner == player).FirstOrDefault();

        ship.photonView.RPC(Spaceship.RPC_SET_SPECIAL, RpcTarget.All, WeaponEnum);

        gameObject.SetActive(false);
    }

    public static string RPC_SPAWN_WEAPON_PICKUP = "RPC_SpawnWeaponPickup";
    [PunRPC]
    private void RPC_ActivateWeaponPickup(WeaponEnum weapon)
    {
        WeaponEnum = weapon;
        gameObject.SetActive(true);
    }

    

}
