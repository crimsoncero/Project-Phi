using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Lazgun", menuName = "Scriptable Objects/Weapons/Lazgun")]
public class Lazgun : Weapon
{
    [SerializeField] private Vector3 _spawnPoint;

    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag)
    {
        Projectile projectile = Instantiate(ProjectilePrefab, AdjustPosition(_spawnPoint, shipPosition, shipRotation), Quaternion.identity);
        projectile.Init(photonView.Owner, Damage, AdjustVelocity(shipVelocity, shipRotation) , shipRotation, lag);
    }
}
