using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Proximity Mine", menuName = "Scriptable Objects/Weapons/Proximity Mine")]
public class ProximityMine : Weapon
{
    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag)
    {
        throw new System.NotImplementedException();
    }
}
