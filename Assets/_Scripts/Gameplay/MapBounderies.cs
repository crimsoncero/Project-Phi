using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(EdgeCollider2D))]
public class CullingBox : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const string ProjectileTag = "Projectile";


    [SerializeField] private EdgeCollider2D _cullingBoxCollider;
    [SerializeField] private EdgeCollider2D _mapBorderCollider;
    [SerializeField] private EdgeCollider2D _mapReturnCollider;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private float _distanceToCulling;
    [SerializeField] private float _distanceToReturn;


    [Header("Debug")]
    [SerializeField] private Color _cullingColor;
    [SerializeField] private Color _borderColor;
    [SerializeField] private Color _returnColor;
    [SerializeField] private bool _showCulling;
    [SerializeField] private bool _showBorder;
    [SerializeField] private bool _showReturn;

    
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
        _mapBorderCollider.points = FindVertices(_mapSize, 0);
        _cullingBoxCollider.points = FindVertices(_mapSize, _distanceToCulling);
        _mapReturnCollider.points = FindVertices(_mapSize, -_distanceToReturn);
    }

    /// <summary>
    /// Returns an array to define a box edge collider.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="adjustment"></param>
    /// <returns></returns>
    private Vector2[] FindVertices(Vector2 size, float adjustment)
    {
        Vector2 position = transform.position;

        size.x += adjustment;
        size.y += adjustment;

        List<Vector2> vertices = new List<Vector2>()
        {
            new Vector2(size.x / 2 - position.x, size.y / 2 - position.y),
            new Vector2(size.x / 2 - position.x, -size.y / 2 - position.y),
            new Vector2(-size.x / 2 - position.x, -size.y / 2 - position.y),
            new Vector2(-size.x / 2 - position.x, size.y / 2 - position.y),
            new Vector2(size.x / 2 - position.x, size.y / 2 - position.y)
        };

        return vertices.ToArray();
    }

    private void OnDrawGizmos()
    {
        Vector3 size = _mapSize;

        if (_showBorder)
        {
            Gizmos.color = _borderColor;
            Gizmos.DrawWireCube(Vector3.zero, size);
        }

        if (_showCulling)
        {
            Gizmos.color = _cullingColor;
            size.x = _mapSize.x + _distanceToCulling;
            size.y = _mapSize.y + _distanceToCulling;
            Gizmos.DrawWireCube(Vector3.zero, size);
        }

        if (_showReturn)
        {
            Gizmos.color = _returnColor;
            size.x = _mapSize.x - _distanceToReturn;
            size.y = _mapSize.y - _distanceToReturn;
            Gizmos.DrawWireCube(Vector3.zero, size);
        }

    }

}
