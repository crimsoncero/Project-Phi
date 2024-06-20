using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Lazgun", menuName = "Scriptable Objects/Weapons/Lazgun")]
public class Lazgun : Weapon
{
    [SerializeField] private Vector3 _spawnPoint;
    [SerializeField] private Projectile _laser;

    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, float shipVelocity)
    {
        Vector3 originalDirection = shipRotation * Vector2.up;


        Projectile projectile = Instantiate(_laser, AdjustPosition(_spawnPoint, shipPosition, shipRotation), Quaternion.identity);
        projectile.InitProjectile(Velocity + shipVelocity, originalDirection, 0);
    }
}
