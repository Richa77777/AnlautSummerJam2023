using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Vector2 direction = -Player.Instance.GetPlayerShootingComponent.GetShootPoint.up;
        _rigidbody.velocity = direction * _speed;
    }
}
