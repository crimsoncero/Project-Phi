using Photon.Pun;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Autocannon", menuName = "Scriptable Objects/Weapons/Autocannon")]
public class Autocannon : Weapon
{
    [SerializeField] private Vector3 _leftSpawn;
    [SerializeField] private Vector3 _rightSpawn;

    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag, int currentAmmo)
    {
        Instantiate(ProjectilePrefab, AdjustPosition(_leftSpawn, shipPosition, shipRotation), Quaternion.identity)
            .Init(photonView.Owner, Damage, AdjustVelocity(shipVelocity, shipRotation), shipRotation, lag);

        Instantiate(ProjectilePrefab, AdjustPosition(_rightSpawn, shipPosition, shipRotation), Quaternion.identity)
            .Init(photonView.Owner, Damage, AdjustVelocity(shipVelocity, shipRotation), shipRotation, lag);
    }

    
}
