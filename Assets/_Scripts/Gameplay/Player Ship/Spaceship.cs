using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviourPun, IPunObservable
{
    public event Action<float> OnHeatChanged;
    public event Action OnSpecialFired;
    public event Action OnHealthChanged;
    public event Action OnSpecialChanged;
    public event Action OnSpawn;
    public event Action OnDestroyed;
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public Lazgun PrimaryWeapon { get; private set; }
    [field: SerializeField] public Weapon SpecialWeapon { get; private set; }
    [SerializeField] private float _globalCooldown;
    [SerializeField] private float _immuneTime;

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private WeaponAnimator _weaponAnimator;
    [SerializeField] private PlayerController _playerController;
    public PlayerController PlayerController { get { return _playerController; } }
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private ShipFeedbacks _shipFeedbacks;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _shipAnimator;
    [SerializeField] private Explosion _explosionPrefab;
    
    
    // Health
    private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set 
        { 
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke();

            if (photonView.IsMine)
                UpdateAnimator();
        }
    }

    public bool IsImmune { get; private set; }

    // Heat and Ammo
    public bool IsOverHeating { get; private set; }
    private Coroutine _cooldownRoutine;
    private float _primaryHeat;
    public float PrimaryHeat
    {
        get { return _primaryHeat; }
        set { _primaryHeat = Mathf.Clamp(value, 0, PrimaryWeapon.MaxHeat); }
    }
    private int _specialAmmo;
	public int SpecialAmmo
	{
		get { return _specialAmmo; }
		set { _specialAmmo = Mathf.Clamp(value, 0, SpecialWeapon.MaxAmmo); }
	}
    
    // Fire Control

    public bool CanGlobalFire { get; private set; } = true;
    public bool CanPrimaryFire { get; private set; } = true;
    public bool CanSpecialFire { get; private set; } = true;

    private SpaceshipConfig _config;
    public SpaceshipConfig Config { get { return _config; } }

    private void Awake()
    {
        GameManager.Instance.RegisterSpaceship(this);
    }
    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            SetInputActive(false);
        }
        if (_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);
    }

    #region Pun RPC

    /* Method names are prefixed with RPC_MethodName
     * Before every RPC method add a public string const of the method name.
     * string names are formatted with uppercase and '_' between words:  RPC_METHOD_NAME.
     */

    public const string RPC_FIRE_PRIMARY = "RPC_FirePrimary";
    [PunRPC]
    private void RPC_FirePrimary(Vector3 position, Quaternion rotation,  Vector2 velocity, PhotonMessageInfo info)
    {
        // Reduce overhead in other clients, cooldown tracking is only useful for special weapons.
        if (photonView.IsMine && gameObject.activeSelf)
            StartCoroutine(WaitForCanFire(true));

        if(_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);

        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        PrimaryWeapon.Fire(photonView, position, rotation, velocity, lag, (int)PrimaryHeat);
        PrimaryHeat += PrimaryWeapon.HeatPerShot;
        OnHeatChanged?.Invoke(PrimaryHeat/PrimaryWeapon.MaxHeat);
        if (gameObject.activeSelf)
            _cooldownRoutine = StartCoroutine(CooldownPrimary());

        // Feedback

        _shipFeedbacks.FireWeaponSFX(PrimaryWeapon, photonView.IsMine);
    }

    public const string RPC_FIRE_SPECIAL = "RPC_FireSpecial";
    [PunRPC]
    private void RPC_FireSpecial(Vector3 position, Quaternion rotation, Vector2 velocity, PhotonMessageInfo info)
    {
        if (gameObject.activeSelf)
            StartCoroutine(WaitForCanFire(false));

        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        SpecialWeapon.Fire(photonView, position, rotation, velocity, lag, SpecialAmmo);
        SpecialAmmo--;

        _weaponAnimator.FireAnim();

        if (SpecialAmmo == 0)
            photonView.RPC(RPC_CLEAR_SPECIAL, RpcTarget.All);
        
        
        // Feedback
        _shipFeedbacks.FireWeaponSFX(SpecialWeapon, photonView.IsMine);
        OnSpecialFired?.Invoke();

    }

    public const string RPC_SET_SPECIAL = "RPC_SetSpecial";
    [PunRPC]
    private void RPC_SetSpecial(WeaponEnum weaponEnum)
    {
        SpecialWeapon = GameManager.Instance.WeaponList.GetWeapon(weaponEnum);
        SpecialAmmo = SpecialWeapon.MaxAmmo;
        _weaponAnimator.SetWeapon(SpecialWeapon);
        OnSpecialChanged?.Invoke();
    }

    public const string RPC_CLEAR_SPECIAL = "RPC_ClearSpecial";
    [PunRPC]
    private void RPC_ClearSpecial()
    {
        if (gameObject.activeSelf)
            StartCoroutine(ClearSpecial());

    }

    public const string RPC_HIT = "RPC_Hit";
    [PunRPC]
    private void RPC_Hit(HitData hitData)
    {
        
        
        if (IsImmune)
            return;
        if(photonView.IsMine)
            TakeDamage(hitData);
    }

    public const string RPC_DESTROYED = "RPC_Destroyed";
    [PunRPC]
    private void RPC_Destroyed(HitData hitData)
    {
        // Start ship respawn and raise player score
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.IncreasePlayerScore(hitData.Owner, 1);
            GameManager.Instance.SpawnShip(this, true);
        }
        OnDestroyed?.Invoke();
        Instantiate(_explosionPrefab, transform.position, transform.rotation);
        this.gameObject.SetActive(false);

    }

    /// <summary>
    /// Spawns the ship in the designated location and activates it.
    /// </summary>
    public const string RPC_SPAWN = "RPC_Spawn";
    [PunRPC]
    private void RPC_Spawn(Vector3 position, Quaternion rotation, int spawnTime, PhotonMessageInfo info)
    {
        
        transform.position = position;
        transform.rotation = rotation;

        GameManager.Instance.DelaySpawnShip(this, spawnTime);
    
    }

    public const string RPC_ACTIVATE = "RPC_Activate";
    [PunRPC]
    private void RPC_Activate()
    {
        GameManager.Instance.RegisterSpaceship(this);
        gameObject.SetActive(true);
    }
    #endregion

    public IEnumerator DelayedSpawn(int spawnTime)
    {
        int delta = spawnTime - PhotonNetwork.ServerTimestamp;
        // Wait for server time to pass spawn time (optimize for lowest wait time between checks)
        
        while(delta > 0)
        {
            if (!GameManager.Instance.IsMatchActive) yield break;
            
            yield return new WaitForSeconds(0.1f);
            delta = spawnTime - PhotonNetwork.ServerTimestamp;
        }

        // Spawn ship
        if (GameManager.Instance.IsMatchActive)
        {
            gameObject.SetActive(true);

            if (photonView.IsMine)
                SetInputActive(true);

            float lag = delta * -0.001f;
            if (gameObject.activeSelf)
                StartCoroutine(ImmuneCoroutine(lag));

        }
    }

    /// <summary>
    /// Initialize Spaceship stats to default starting values.
    /// </summary>
    public void Initialize()
    {
        
        if (PrimaryWeapon != null)
            PrimaryHeat = 0;

        SpecialWeapon = null;
        CanGlobalFire = true;
        CanPrimaryFire = true;
        CanSpecialFire = true;
        CurrentHealth = MaxHealth;
        OnSpawn?.Invoke();
        if (photonView.IsMine)
        {
            SetInputActive(false);
            SetInputActive(true);
        }

    }

    /// <summary>
    /// sets the input and player controller active or deactive. Only works if localplayer is the owner.
    /// </summary>
    /// <param name="activate"></param>
    public void SetInputActive(bool activate)
    {
        if (!photonView.IsMine) return; // Not mine
        if (photonView.IsRoomView) return; // Technically mine, but not really.

        _playerController.enabled = activate;
        _playerInput.enabled = activate;
    }

    public void FlashInput()
    {
        SetInputActive(false);
        SetInputActive(true);
    }
    /// <summary>
    /// Sets the ship config, if no config is given uses the owner player config.
    /// </summary>
    /// <param name="config"></param>
    public void SetConfig(SpaceshipConfig config = null)
    {
        if (config == null)
        {
            if (!PhotonNetwork.OfflineMode)
                _config = GameManager.Instance.ShipConfigList.GetPlayerConfig(photonView.Owner);
            else
                _config = GameManager.Instance.ShipConfigList.GetConfig(0);
        }
        else
            _config = config;

        _renderer.material = _config.Material;
    }

    public void SetSpecial(Weapon weapon)
    {
        photonView.RPC(RPC_SET_SPECIAL, RpcTarget.All, weapon);
    } 
    
    public IEnumerator ClearSpecial()
    {
        yield return new WaitUntil(() => CanSpecialFire);
        _weaponAnimator.SetWeapon(null);
        SpecialWeapon = null;
        OnSpecialChanged?.Invoke();

    }

    private void TakeDamage(HitData hitData)
    {
        if (hitData.Damage <= 0) return;
        CurrentHealth -= hitData.Damage;

        if (CurrentHealth <= 0)
            photonView.RPC(RPC_DESTROYED, RpcTarget.All, hitData);
    }

    private IEnumerator CooldownPrimary()
    {
        // Wait before cooling down, duration dependent if overheating or not. 
        if(PrimaryHeat >= PrimaryWeapon.MaxHeat)
            IsOverHeating = true;
        
        yield return new WaitForSeconds(PrimaryWeapon.TimeToCool);

        float duration = 0;
        while (PrimaryHeat > 0)
        {
            float time = Time.deltaTime;
            PrimaryHeat -= PrimaryWeapon.CalcCooling(duration, time);
            OnHeatChanged?.Invoke(PrimaryHeat/PrimaryWeapon.MaxHeat);
            duration += time;
            yield return null;
        }
        if(PrimaryHeat == 0)
            IsOverHeating = false;
    }
    private IEnumerator WaitForCanFire(bool isPrimary)
    {
        Weapon weaponFired = isPrimary ? PrimaryWeapon : SpecialWeapon;

        if (isPrimary)
        {
            CanPrimaryFire = false;
        }
        else
            CanSpecialFire = false;
        if (gameObject.activeSelf)
            StartCoroutine(WaitForGCD());

        yield return new WaitForSeconds(weaponFired.TimeBetweenShots);

        if (isPrimary)
            CanPrimaryFire = true;
        else
            CanSpecialFire = true;


        if (photonView.IsMine)
        {
            // Autofire
            if (weaponFired.FiringMethod == Weapon.FiringMethods.Auto)
            {
                if (isPrimary)
                    _playerController.FirePrimary();
                else
                    _playerController.FireSpecial();
            }
        }
    }

    private IEnumerator WaitForGCD()
    {
        CanGlobalFire = false;
        yield return new WaitForSeconds(_globalCooldown);
        CanGlobalFire = true;
    }

    private void UpdateAnimator()
    {
        if (!photonView.IsMine) return;

        // Check Health State:
        int healthState = 0;

        if (_currentHealth > 75)
            healthState = 0;
        else if(_currentHealth > 50)
            healthState = 1;
        else if(_currentHealth > 25)
            healthState = 2;
        else
            healthState = 3;

        _shipAnimator.SetInteger("HealthState", healthState);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send Data
            stream.SendNext(CurrentHealth); // HP Sync
        }
        else
        {
            // Reciece Data
            CurrentHealth = (int)stream.ReceiveNext(); // HP Sync
        }
    }

    private IEnumerator ImmuneCoroutine(float lag)
    {
        IsImmune = true;
        _shipAnimator.SetBool("IsImmune", true);

        // reduce the wait time with the time it took to recieve the message.
        float timeAdjusted = _immuneTime - lag;

        if(timeAdjusted > 0)
            yield return new WaitForSeconds(timeAdjusted);

        IsImmune = false;
        _shipAnimator.SetBool("IsImmune", false);
    }
}
