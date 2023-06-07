using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody2D _rigidbody;
        private BoxCollider2D _collider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void Move()
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            _rigidbody.velocity = new Vector3(horizontalAxis * _moveSpeed, _rigidbody.velocity.y, 0f);
        }

        private void Jump()
        {
            if (IsGrounded() == true)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, 0f);
            }
        }

        private bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, 0.1f, _groundLayer);

            return hit.collider != null;
        }
    }
}
