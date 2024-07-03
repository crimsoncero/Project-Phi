using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class Spaceship : MonoBehaviourPun
{
    public event Action<float> OnHeatChanged;
    public event Action OnHealthChanged;

    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public Lazgun PrimaryWeapon { get; private set; }
    [field: SerializeField] public Weapon SpecialWeapon { get; private set; }
    [SerializeField] private float _globalCooldown;

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private WeaponAnimator _weaponAnimator;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private WeaponList _weaponList;
    [SerializeField] private ShipFeedbacks _shipFeedbacks;
    [SerializeField] private SpriteRenderer _renderer;

    // Health
    private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set 
        { 
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke();
        }
    }


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

    private void Awake()
    {
        GameManager.Instance.RegisterSpaceship(this);
    }
    private void OnEnable()
    {
        Init();
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
        if (photonView.IsMine)
            StartCoroutine(WaitForCanFire(true));

        if(_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);

        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        PrimaryWeapon.Fire(photonView, position, rotation, velocity, lag, (int)PrimaryHeat);
        PrimaryHeat += PrimaryWeapon.HeatPerShot;
        OnHeatChanged?.Invoke(PrimaryHeat/PrimaryWeapon.MaxHeat);
        _cooldownRoutine = StartCoroutine(CooldownPrimary());

    }

    public const string RPC_FIRE_SPECIAL = "RPC_FireSpecial";
    [PunRPC]
    private void RPC_FireSpecial(Vector3 position, Quaternion rotation, Vector2 velocity, PhotonMessageInfo info)
    {
        StartCoroutine(WaitForCanFire(false));

        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        SpecialWeapon.Fire(photonView, position, rotation, velocity, lag, SpecialAmmo);
        SpecialAmmo--;

        _weaponAnimator.FireAnim();

        if (SpecialAmmo == 0)
            photonView.RPC(RPC_CLEAR_SPECIAL, RpcTarget.All);
    }

    public const string RPC_SET_SPECIAL = "RPC_SetSpecial";
    [PunRPC]
    private void RPC_SetSpecial(WeaponEnum weaponEnum)
    {
        SpecialWeapon = _weaponList.GetWeapon(weaponEnum);
        SpecialAmmo = SpecialWeapon.MaxAmmo;
        _weaponAnimator.SetWeapon(SpecialWeapon);
    }

    public const string RPC_CLEAR_SPECIAL = "RPC_ClearSpecial";
    [PunRPC]
    private void RPC_ClearSpecial()
    {
        StartCoroutine(ClearSpecial());
    }
    public const string RPC_HIT = "RPC_Hit";
    [PunRPC]
    private void RPC_Hit(int damage, Player owner, Vector3 position)
    {
        TakeDamage(damage);
    }

    #endregion


    public void Init()
    {
        if (SpecialWeapon != null)
            SpecialAmmo = SpecialWeapon.MaxAmmo;
        if (PrimaryWeapon != null)
            PrimaryHeat = 0;

        CurrentHealth = MaxHealth;
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
    }

    private void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        CurrentHealth -= damage;
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

}
