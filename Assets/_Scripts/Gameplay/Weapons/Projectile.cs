using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player Owner { get; private set; }
    public int Damage { get; private set; }

    [Tooltip("The maximum distance to snap the projectile to its expected position.")]
    [SerializeField] private float _distanceToSnap = 0.1f;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private float _multiplier = 0.7f;

    public void Init(Player owner, int damage, float velocity, Quaternion shipRotation, float lag)
    {
        Vector3 originalDirection = shipRotation * Vector2.up;

        Owner = owner;
        Damage = damage;

        transform.up = originalDirection;

        StartCoroutine(LatencyCatchup(originalDirection * velocity, transform.position, lag));
    }


    private IEnumerator LatencyCatchup(Vector2 baseVelocity, Vector2 startingPosition, float lag)
    {

        while (true)
        {
            // Calculate the expected position of the projectile based on latency, and the distance from the current position.
            Vector3 expectedPosition = startingPosition + (baseVelocity * lag);
            float distanceFromExpected = Vector3.Distance(expectedPosition, transform.position);

            // Snap the position and end the catchup if below the threshold. 
            if(distanceFromExpected < _distanceToSnap)
            {
                _rigidbody2D.velocity = baseVelocity;
                transform.position = expectedPosition;
                yield break;
            }

            float velocityMultiplier = Mathf.Log(_multiplier * distanceFromExpected + MathF.E, MathF.E);

            _rigidbody2D.velocity = baseVelocity * velocityMultiplier;


            // Wait for next frame and aggregate the lag.
            yield return null;
            lag += Time.deltaTime;
        }
    }
}
