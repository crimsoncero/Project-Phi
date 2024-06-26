using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Animator _autocannon;
    [SerializeField] private Animator _hellfireRocketPod;
    [SerializeField] private Animator _voidMineDispenser;
    [SerializeField] private Animator _doomCannon;

    public Animator CurrentWeapon { get; private set; }

    private void Awake()
    {
        _autocannon.gameObject.SetActive(false);
        _hellfireRocketPod.gameObject.SetActive(false);
        _voidMineDispenser.gameObject.SetActive(false);
        _doomCannon.gameObject.SetActive(false);
    }



    public void SetWeapon()
    {

    }


}
