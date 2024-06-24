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
    private void FirePrimary(Vector3 position, Quaternion rotation,  float velocityMag, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        PrimaryWeapon.Fire(_photonView, position, rotation, velocityMag, lag);
    }

    [PunRPC]
    private void FireSpecial(Vector3 position, Quaternion rotation, float velocityMag, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        SpecialWeapon.Fire(_photonView, position, rotation, velocityMag, lag);
        SpecialAmmo--;

    }
    #endregion
}
