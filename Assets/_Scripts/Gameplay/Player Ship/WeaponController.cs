using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Animator _autocannon;
    [SerializeField] private Animator _hellfireRocketPod;
    [SerializeField] private Animator _voidMineDispenser;
    [SerializeField] private Animator _doomLaserCannon;
    public Animator CurrentWeaponAnim { get; private set; }

    private Weapon _currentWeapon = null;
    private void Awake()
    {
        _autocannon.gameObject.SetActive(false);
        _hellfireRocketPod.gameObject.SetActive(false);
        _voidMineDispenser.gameObject.SetActive(false);
        _doomLaserCannon.gameObject.SetActive(false);
    }


    /// <summary>
    /// Sets the current weapon controlled.
    /// </summary>
    public void SetWeapon(Weapon weapon)
    {
        // Disable previous weapon visuals
        if(!CurrentWeaponAnim.IsUnityNull())
            CurrentWeaponAnim.gameObject.SetActive(false);

        if (weapon == null)
        {
            CurrentWeaponAnim = null;
            _currentWeapon = null;
            return;
        }

        _currentWeapon = weapon;
        
        switch (weapon)
        {
            case Autocannon:
                CurrentWeaponAnim = _autocannon;
                break;
            case RocketPod:
                CurrentWeaponAnim = _hellfireRocketPod;
                break;
            case ProximityMine:
                CurrentWeaponAnim = _voidMineDispenser;
                break;
            case DoomLaser:
                CurrentWeaponAnim = _doomLaserCannon;
                break;

        }
        CurrentWeaponAnim.gameObject.SetActive(true);
    }

    /// <summary>
    /// Play the fire animation of the current weapon in use.
    /// </summary>
    public void FireAnim()
    {
        // Only Trigger the animation if owner.
        if (!_photonView.IsMine)
            return;

        switch (_currentWeapon)
        {
            case Autocannon:
                _autocannon.SetTrigger("Fire"); 
                break;
            case RocketPod:
                CurrentWeaponAnim = _hellfireRocketPod;
                break;
            case ProximityMine:
                CurrentWeaponAnim = _voidMineDispenser;
                break;
            case DoomLaser:
                CurrentWeaponAnim = _doomLaserCannon;
                break;

        }
    }

    

}
