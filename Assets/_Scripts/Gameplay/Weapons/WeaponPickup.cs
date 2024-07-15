using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer _weaponRenderer;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private Sprite _autoCannonSprite;
    [SerializeField] private Sprite _rocketPodSprite;
    [SerializeField] private Sprite _mineDispenserSprite;
    [SerializeField] private Sprite _doomLaserSprite;

    [field: SerializeField] public WeaponEnum WeaponEnum { get; private set; }
    public bool IsAvailable { get; private set; } = true;

    [SerializeField] private InputActionAsset _inputAsset;
    private InputActionMap _inputMap;
    private InputAction _onInteract;

    private void Start()
    {
        _inputMap = _inputAsset.FindActionMap("Player");
        _onInteract = _inputMap.FindAction("Interact");
    }

    private void OnEnable()
    {
        _canvas.enabled = false;
        IsAvailable = true;
    }

    private void OnDisable()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return; // Only consider collisions with player objects.
        if (other.gameObject != GameManager.Instance.ClientSpaceship.gameObject) return; // Only consider collisions with the clients spaceship.

        _onInteract.performed += OnInteract;
        _canvas.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return; // Only consider collisions with player objects.
        if (other.gameObject != GameManager.Instance.ClientSpaceship.gameObject) return; // Only consider collisions with the clients spaceship.

        _onInteract.performed -= OnInteract;
        _canvas.enabled = false;

    }

    public void OnInteract(InputAction.CallbackContext obj)
    {
         
        if (!IsAvailable) return;
        Debug.Log(photonView);
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

        GameManager.Instance.WeaponPickedUp(this);
    }

    public static string RPC_DEACTIVATE = "RPC_Deactivate";
    [PunRPC]
    private void RPC_Deactivate()
    {
        gameObject.SetActive(false);
    }

    public static string RPC_ACTIVATE_WEAPON_PICKUP = "RPC_ActivateWeaponPickup";
    [PunRPC]
    private void RPC_ActivateWeaponPickup(WeaponEnum weapon)
    {
        WeaponEnum = weapon;

        switch (weapon)
        {
            case WeaponEnum.Autocannon:
                _weaponRenderer.sprite = _autoCannonSprite;
                break;
            case WeaponEnum.RocketPod:
                _weaponRenderer.sprite = _rocketPodSprite;
                break;
            case WeaponEnum.MineDispenser:
                _weaponRenderer.sprite = _mineDispenserSprite;
                break;
            case WeaponEnum.DoomLaser:
                _weaponRenderer.sprite = _doomLaserSprite;
                break;
        }

        gameObject.SetActive(true);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;

        if ((bool)data[0])
        {
            RPC_ActivateWeaponPickup((WeaponEnum)data[1]);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                GameManager.Instance.WeaponPickedUp(this, true);

            gameObject.SetActive(false);
        }


    }
}