using Photon.Pun;
using Photon.Realtime;
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


    [Header("Attributes")]

    [field: SerializeField]
    public FiringMethods FiringMethod { get; private set; }
    [field: SerializeField]
    public int MaxAmmo { get; private set; }

    [field: SerializeField]
    public float Cooldown { get; private set; }
    
    [field: SerializeField]
    public float Velocity { get; private set; }


    public abstract void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, float shipVelocity, float lag);


    protected Vector3 AdjustPosition(Vector3 point, Vector3 shipPosition, Quaternion shipRotation)
    {
        Vector3 rotatedPoint = shipRotation * (point) + shipPosition;
        return rotatedPoint;
    }
}
