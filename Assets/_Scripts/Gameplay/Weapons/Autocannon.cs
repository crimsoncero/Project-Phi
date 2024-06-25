using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Autocannon", menuName = "Scriptable Objects/Weapons/Autocannon")]
public class Autocannon : Weapon
{
    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag)
    {
        throw new System.NotImplementedException();
    }
}
