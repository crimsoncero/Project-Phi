using Photon.Pun;
using UnityEngine;


public abstract class Weapon : ScriptableObject
{
    public enum FiringMethods
    {
        Single,
        Auto,
        Beam,
    }

    [field: Header("Weapon Attributes")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public FiringMethods FiringMethod { get; private set; }
    [field: SerializeField] public int MaxAmmo { get; private set; }
    [field: SerializeField] public float FireRate { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public GameObject WeaponPrefab { get; set; }


    [Space]
    [Header("Projectile Attributes")]
    [field: SerializeField] public Projectile ProjectilePrefab;
    [field: SerializeField] public float ProjectileVelocity { get; private set; }


    public float TimeBetweenShots { get { return 1 / FireRate; } }
    public abstract void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag, int currentAmmo);


    protected Vector3 AdjustPosition(Vector3 point, Vector3 shipPosition, Quaternion shipRotation)
    {
        Vector3 rotatedPoint = (shipRotation * point) + shipPosition;
        return rotatedPoint;
    }

    protected float AdjustVelocity(Vector2 shipVelocity, Quaternion shipRotation)
    {

        Vector3 direction = shipRotation * Vector2.up;

        float angle = Mathf.Acos((Vector3.Dot(direction, shipVelocity)) / (direction.magnitude * shipVelocity.magnitude));

        if(shipVelocity.magnitude < 0.2f)
            return ProjectileVelocity;

        if (Mathf.Abs(angle) <= Mathf.PI / 3)
        {
            return shipVelocity.magnitude / 2 + ProjectileVelocity;
        }
        else
        {
            return ProjectileVelocity;
        }
    }

    /// <summary>
    /// Easy access to the projectile pool get projectile.
    /// </summary>
    /// <returns></returns>
    protected Projectile GetProjectile()
    {
        return ProjectilePool.Instance.GetProjectile(this);
    }
}
