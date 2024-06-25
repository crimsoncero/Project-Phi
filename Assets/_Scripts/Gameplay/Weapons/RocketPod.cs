using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Rocket Pod", menuName = "Scriptable Objects/Weapons/Rocket Pod")]
public class RocketPod : Weapon
{
    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag)
    {
        throw new System.NotImplementedException();
    }
}
