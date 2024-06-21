using Photon.Pun;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public Weapon PrimaryWeapon { get; private set; }
    [field: SerializeField] public Weapon SpecialWeapon { get; private set; }

    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Rigidbody2D _rigidbody2D;

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


    #region Pun RPC
    [PunRPC] // NEED TO ADD Current Position and rotation and velocity of the ship when message was sent
    private void FirePrimary(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        PrimaryWeapon.Fire(_photonView, transform.position, transform.rotation, _rigidbody2D.velocity.magnitude, lag);
    }

    [PunRPC]
    private void FireSpecial(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        SpecialWeapon.Fire(_photonView, transform.position, transform.rotation, _rigidbody2D.velocity.magnitude, lag);
        SpecialAmmo--;
    }
    #endregion
}
