using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public Lazgun PrimaryWeapon { get; private set; }
    [field: SerializeField] public Weapon SpecialWeapon { get; private set; }

    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameObject _visuals;


    // Heat and Ammo
    public bool IsOverHeating { get; private set; }
    private Coroutine _cooldownRoutine;
    private float _primaryHeat;
    public float PrimaryHeat
    {
        get { return _primaryHeat; }
        set { _primaryHeat = Mathf.Clamp(value, 0, PrimaryWeapon.MaximumHeat); }
    }
    private int _specialAmmo;
	public int SpecialAmmo
	{
		get { return _specialAmmo; }
		set { _specialAmmo = Mathf.Clamp(value, 0, SpecialWeapon.MaxAmmo); }
	}

    private void OnEnable()
    {
        if(SpecialWeapon != null)
            SpecialAmmo = SpecialWeapon.MaxAmmo;
        if (PrimaryWeapon != null)
            PrimaryHeat = 0;

    }

    #region Pun RPC
    [PunRPC] // NEED TO ADD Current Position and rotation and velocity of the ship when message was sent
    private void FirePrimary(Vector3 position, Quaternion rotation,  Vector2 velocity, PhotonMessageInfo info)
    {
        if(_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);

        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        PrimaryWeapon.Fire(_photonView, position, rotation, velocity, lag, (int)PrimaryHeat);
        PrimaryHeat += 1f;

        _cooldownRoutine = StartCoroutine(CooldownPrimary());

    }

    [PunRPC]
    private void FireSpecial(Vector3 position, Quaternion rotation, Vector2 velocity, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        SpecialWeapon.Fire(_photonView, position, rotation, velocity, lag, SpecialAmmo);
        SpecialAmmo--;
    }
    #endregion

    public void SetSpecialWeapon(Weapon weapon)
    {
        SpecialWeapon = weapon;

        Instantiate(SpecialWeapon.WeaponPrefab, _visuals.transform);
    }

    private IEnumerator CooldownPrimary()
    {
        // Wait before cooling down, duration dependent if overheating or not. 
        if(PrimaryHeat >= PrimaryWeapon.MaximumHeat)
        {
            IsOverHeating = true;
            yield return new WaitForSeconds(PrimaryWeapon.OverheatPenalty);
            IsOverHeating = false;
        }
        else
        {
            yield return new WaitForSeconds(PrimaryWeapon.TimeToCool);
        }

        float duration = 0;
        while (PrimaryHeat > 0)
        {
            PrimaryHeat -= PrimaryWeapon.CalcCooling(duration, Time.deltaTime);
            duration += Time.deltaTime;
            yield return null;
        }

    }

}
