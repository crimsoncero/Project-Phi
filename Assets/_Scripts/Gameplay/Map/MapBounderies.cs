using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class CullingBox : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const string ProjectileTag = "Projectile";


    [SerializeField] private EdgeCollider2D _cullingBoxCollider;
    [SerializeField] private EdgeCollider2D _mapBoundsCollider;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private float _distanceToCulling;

    
    private void Awake()
    {
        DefineColliders();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ProjectileTag)
            other.gameObject.SetActive(false);
        else
            Debug.Log("Ship OOB");
    }

    private void OnValidate()
    {
        DefineColliders();
    }

    private void DefineColliders()
    {
        Vector2 position = transform.position;
        List<Vector2> mapVertices = new List<Vector2>()
        {
            new Vector2(_mapSize.x / 2 - position.x, _mapSize.y / 2 - position.y),
            new Vector2(_mapSize.x / 2 - position.x, -_mapSize.y / 2 - position.y),
            new Vector2(-_mapSize.x / 2 - position.x, -_mapSize.y / 2 - position.y),
            new Vector2(-_mapSize.x / 2 - position.x, _mapSize.y / 2 - position.y),
            new Vector2(_mapSize.x / 2 - position.x, _mapSize.y / 2 - position.y)
        };
        _mapBoundsCollider.points = mapVertices.ToArray();

        Vector2 size = new Vector2(_mapSize.x + _distanceToCulling, _mapSize.y + _distanceToCulling);

        List<Vector2> cullingVertices = new List<Vector2>()
        {
            new Vector2(size.x / 2 - position.x, size.y / 2 - position.y),
            new Vector2(size.x / 2 - position.x, -size.y / 2 - position.y),
            new Vector2(-size.x / 2 - position.x, -size.y / 2 - position.y),
            new Vector2(-size.x / 2 - position.x, size.y / 2 - position.y),
            new Vector2(size.x / 2 - position.x, size.y / 2 - position.y)
        };
        _cullingBoxCollider.points = cullingVertices.ToArray();
    }
   
}
