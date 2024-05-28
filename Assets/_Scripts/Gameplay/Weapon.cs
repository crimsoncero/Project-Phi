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

    public bool CanFire { get; set; }

    public abstract void Fire();
}
