using Photon.Realtime;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player Owner { get; set; }

    [SerializeField] private Rigidbody2D _rigidbody;



    public void InitProjectile(Player owner, float velocity, Vector3 originalDirection, float lag)
    {
        transform.up = originalDirection;

        _rigidbody.velocity = originalDirection * velocity;
        _rigidbody.position += _rigidbody.velocity * lag;
    }
}
