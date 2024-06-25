using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Doom Laser", menuName = "Scriptable Objects/Weapons/Doom Laser")]
public class DoomLaser : Weapon
{
    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag)
    {
        throw new System.NotImplementedException();
    }
}
