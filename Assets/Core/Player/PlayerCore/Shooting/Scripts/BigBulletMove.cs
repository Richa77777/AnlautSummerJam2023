using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletsSpace
{
    public class BigBulletMove : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rigidbody.velocity = Vector2.down * _speed;
        }
    }
}
