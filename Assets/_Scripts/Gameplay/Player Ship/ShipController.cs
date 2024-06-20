using UnityEngine;

public class ShipController : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public Weapon PrimaryWeapon { get; private set; }
    [field: SerializeField] public Weapon SpecialWeapon { get; private set; }



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
    }



}
