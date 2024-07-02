using UnityEngine;

public class MPTester : MonoBehaviour
{
    public Spaceship PlayerShip { get { return GameManager.Instance.ClientSpaceship; } }

    public void SetWeapon(int weaponEnum)
    {
        PlayerShip.photonView.RPC(Spaceship.RPC_SET_SPECIAL, Photon.Pun.RpcTarget.All, (WeaponEnum)weaponEnum);
    }
}
