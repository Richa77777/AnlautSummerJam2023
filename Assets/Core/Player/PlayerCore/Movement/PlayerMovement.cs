using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _body;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Animator _bodyAnimator;
        private bool _moveBlocked = false;

        public Animator GetBodyAnimator => _bodyAnimator;
        public bool MoveBlockedGet => _moveBlocked;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _bodyAnimator = _body.GetComponent<Animator>();
        }

        private void Update()
        {
            if (_moveBlocked == false)
            {
                Move();
            }

            #region JumpAnimation
            if (IsGrounded())
            {
                if (_bodyAnimator.GetBool("isJumping") == true)
                {
                    _bodyAnimator.SetBool("isJumping", false);
                }
            }
            else if (!IsGrounded())
            {
                float horizontalAxis = Input.GetAxisRaw("Horizontal");

                if (horizontalAxis != 0)
                {
                    if (_bodyAnimator.GetBool("isJumpInDirection") == false)
                    {
                        _bodyAnimator.SetBool("isJumpInDirection", true);
                    }
                }

                else if (horizontalAxis == 0)
                {
                    if (_bodyAnimator.GetBool("isJumpInDirection") == true)
                    {
                        _bodyAnimator.SetBool("isJumpInDirection", false);
                    }
                }

                if (_bodyAnimator.GetBool("isJumping") == false)
                {
                    _bodyAnimator.SetBool("isJumping", true);
                }
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Space) && _moveBlocked == false)
            {
                Jump();
            }
        }

        public void BlockMove()
        {
            _moveBlocked = true;
            StopMove();
        }

        public void UnblockMove()
        {
            _moveBlocked = false;
        }

        private void Move()
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");

            #region WalkAnimation
            if (horizontalAxis != 0)
            {
                if (_bodyAnimator.GetBool("isWalking") == false)
                {
                    _bodyAnimator.SetBool("isWalking", true);
                }
            }

            else if (horizontalAxis == 0)
            {
                if (_bodyAnimator.GetBool("isWalking") == true)
                {
                    _bodyAnimator.SetBool("isWalking", false);
                }
            }
            #endregion

            #region Flip
            if (horizontalAxis > 0)
            {
                _body.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            else if (horizontalAxis < 0)
            {
                _body.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            #endregion

            _rigidbody.velocity = new Vector3(horizontalAxis * _moveSpeed, _rigidbody.velocity.y, 0f);
        }

        private void StopMove()
        {
            _rigidbody.velocity = new Vector3(0f, 0f, 0f);

            if (_bodyAnimator.GetBool("isWalking") == true)
            {
                _bodyAnimator.SetBool("isWalking", false);
            }
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
