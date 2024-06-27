using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Animator _autocannon;
    [SerializeField] private Animator _rocketPod;
    [SerializeField] private Animator _mineDispenser;
    [SerializeField] private Animator _doomLaser;
    public Animator CurrentWeaponAnim { get; private set; }

    private Weapon _currentWeapon = null;
    private void Awake()
    {
        _autocannon.gameObject.SetActive(false);
        _rocketPod.gameObject.SetActive(false);
        _mineDispenser.gameObject.SetActive(false);
        _doomLaser.gameObject.SetActive(false);
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
                CurrentWeaponAnim = _rocketPod;
                break;
            case ProximityMine:
                CurrentWeaponAnim = _mineDispenser;
                break;
            case DoomLaser:
                CurrentWeaponAnim = _doomLaser;
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
                _rocketPod.speed = 1;
                break;
            case ProximityMine:
                break;
            case DoomLaser:
                break;

        }
    }

    

}
