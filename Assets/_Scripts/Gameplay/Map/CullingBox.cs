using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class CullingBox : MonoBehaviour
{
    [SerializeField] private EdgeCollider2D _collider;
    [SerializeField] private Vector2 _size;

    private void Awake()
    {
        if (_collider == null)
        {
            _collider = GetComponent<EdgeCollider2D>();
            _collider.isTrigger = true;
        }

        DefineBox();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        DefineBox();
    }

    private void DefineBox()
    {
        Vector2 position = transform.position;
        List<Vector2> vertices = new List<Vector2>()
        {
            new Vector2(_size.x / 2 - position.x, _size.y / 2 - position.y),
            new Vector2(_size.x / 2 - position.x, -_size.y / 2 - position.y),
            new Vector2(-_size.x / 2 - position.x, -_size.y / 2 - position.y),
            new Vector2(-_size.x / 2 - position.x, _size.y / 2 - position.y),
            new Vector2(_size.x / 2 - position.x, _size.y / 2 - position.y)
        };
        _collider.points = vertices.ToArray();
    }
}
