using Photon.Pun;
using Photon.Realtime;
using System;
using Unity.Mathematics;
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
    [field: SerializeField] public FiringMethods FiringMethod { get; private set; }
    [field: SerializeField] public int MaxAmmo { get; private set; }
    [field: SerializeField] public float FireRate { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public GameObject WeaponPrefab { get; set; }



    [Space]
    [Header("Projectile Attributes")]
    [field: SerializeField] public Projectile ProjectilePrefab;
    [field: SerializeField] public float ProjectileVelocity { get; private set; }


    public abstract void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag, int currentAmmo);


    protected Vector3 AdjustPosition(Vector3 point, Vector3 shipPosition, Quaternion shipRotation)
    {
        Vector3 rotatedPoint = (shipRotation * point) + shipPosition;
        return rotatedPoint;
    }

    protected float AdjustVelocity(Vector2 shipVelocity, Quaternion shipRotation)
    {

        Vector3 direction = shipRotation * Vector2.up;
        float angle = MathF.Atan2(direction.y, direction.x);
        float x = MathF.Cos(angle) * shipVelocity.x;
        float y = MathF.Sin(angle) * shipVelocity.y;

        Vector2 shipVel = new Vector2(x, y);

        float angleBetween = MathF.Acos(Vector2.Dot(direction, shipVelocity) / (direction.magnitude * shipVelocity.magnitude));

        float speed = shipVel.magnitude;
        if (!(angleBetween > 0 && angleBetween < MathF.PI))
            speed *= -1;


        return ProjectileVelocity;
    }

}
