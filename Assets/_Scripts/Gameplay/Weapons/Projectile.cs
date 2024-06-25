using Photon.Realtime;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player Owner { get; private set; }
    public int Damage { get; private set; }

    [SerializeField] private Rigidbody2D _rigidbody;



    public void Init(Player owner, int damage, float velocity, Quaternion shipRotation, float lag)
    {
        Vector3 originalDirection = shipRotation * Vector2.up;

        Owner = owner;
        Damage = damage;

        transform.up = originalDirection;

        _rigidbody.velocity = originalDirection * velocity;
        _rigidbody.position += _rigidbody.velocity * lag;
    }
}
