using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Animator _autocannon;
    [SerializeField] private Animator _hellfireRocketPod;
    [SerializeField] private Animator _voidMineDispenser;
    [SerializeField] private Animator _doomLaserCannon;

    public Animator CurrentWeapon { get; private set; }

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
        if(!CurrentWeapon.IsUnityNull())
            CurrentWeapon.gameObject.SetActive(false);

        if (weapon == null)
        {
            CurrentWeapon = null;
            return;
        }

        switch (weapon.Name)
        {
            case "Autocannon":
                CurrentWeapon = _autocannon;
                break;
            case "Hellfire Rocket Pod":
                CurrentWeapon = _hellfireRocketPod;
                break;
            case "Void Mine Dispenser":
                CurrentWeapon = _voidMineDispenser;
                break;
            case "Doom Laser Cannon":
                CurrentWeapon = _doomLaserCannon;
                break;

        }

        CurrentWeapon.gameObject.SetActive(true);
    }


}
