using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Rocket Pod", menuName = "Scriptable Objects/Weapons/Rocket Pod")]
public class RocketPod : Weapon
{
    [SerializeField] private Vector3 _firstRocket;
    [SerializeField] private Vector3 _secondRocket;
    [SerializeField] private Vector3 _thirdRocket;
    [SerializeField] private Vector3 _fourthRocket;
    [SerializeField] private Vector3 _fifthRocket;
    [SerializeField] private Vector3 _sixthRocket;


    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag, int currentAmmo)
    {
        Vector3 spawn;

        switch (currentAmmo)
        {
            case 6:
                spawn = _firstRocket;
                break;
            case 5:
                spawn = _secondRocket;
                break;
            case 4:
                spawn = _thirdRocket;
                break;
            case 3:
                spawn = _fourthRocket;
                break;
            case 2:
                spawn = _fifthRocket;
                break;
            case 1:
                spawn = _sixthRocket;
                break;
            default: return;
        }

        InitProjectile().Set(AdjustPosition(spawn, shipPosition, shipRotation), photonView.Owner, Damage, AdjustVelocity(shipVelocity, shipRotation), shipRotation, lag);
    }

    private Projectile InitProjectile()
    {
        return GetProjectile().Initialize(ProjectilePool.Instance.RocketPool);
    }
}
