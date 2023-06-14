using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSpace;

namespace PlayerSpace
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform _handWithPistol;
        [SerializeField] private Transform _characterBody;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _bigShootPoint;

        [SerializeField] private float _rotationOffset;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _minRotation = 0f;
        [SerializeField] private float _maxRotation = 150f;

        [SerializeField] private float _smallShotReloadTime = 0.3f;
        [SerializeField] private float _bigShotReloadTime = 1f;
        [SerializeField] private float _bigShotJumpForce = 5f;

        private bool _smallShotAvailable = true;
        private bool _bigShotAvailable = true;
        private bool _shootingAvailable = true;

        private Coroutine _smallShotReloadCor;

        private Rigidbody2D _rigidbody;
        private Animator _characterBodyAnimator;

        public Transform GetHandWithPistol => _handWithPistol;
        public Transform GetShootPoint => _shootPoint;
        public bool ShootingAvailable { set => _shootingAvailable = value; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _characterBodyAnimator = _characterBody.GetComponent<Animator>();
        }

        private void Update()
        {
            SetRotation();

            if (_shootingAvailable == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (_smallShotAvailable == true)
                    {
                        _smallShotAvailable = false;
                        SmallShot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_bigShotAvailable == true)
                    {
                        _bigShotAvailable = false;
                        _handWithPistol.gameObject.SetActive(false);

                        if (_smallShotReloadCor != null)
                        {
                            StopCoroutine(_smallShotReloadCor);
                            _smallShotReloadCor = null;
                        }

                        _smallShotAvailable = false;
                        StartCoroutine(BigShotCor());
                    }
                }
            }
        }

        private void SmallShot()
        {
            GameObject bullet = PoolsController.Instance.GetSmallBulletsPool.GetObjectFromPool();
            bullet.transform.position = _shootPoint.position;
            bullet.transform.rotation = _handWithPistol.transform.rotation;
            bullet.SetActive(true);

            _smallShotReloadCor = StartCoroutine(SmallShotReloading());
        }

        public void BigShot()
        {
            GameObject bullet = PoolsController.Instance.GetBigBulletsPool.GetObjectFromPool();
            bullet.transform.position = _bigShootPoint.transform.position;
            bullet.SetActive(true);
        }

        private IEnumerator BigShotCor()
        {
            _characterBodyAnimator.Play("BigShot", 0, 0);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _bigShotJumpForce);

            yield return new WaitForSeconds(_characterBodyAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.15f);

            _handWithPistol.gameObject.SetActive(true);
            _smallShotAvailable = true;

            StartCoroutine(BigShotReloading());
        }

        private IEnumerator BigShotReloading()
        {
            yield return new WaitForSeconds(_bigShotReloadTime);
            _bigShotAvailable = true;
        }
        private IEnumerator SmallShotReloading()
        {
            yield return new WaitForSeconds(_smallShotReloadTime);
            _smallShotAvailable = true;
        }

        private void SetRotation()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - _handWithPistol.position;

            // ”чтите поворот персонажа при вычислении направлени€
            direction = Quaternion.Euler(0f, -_characterBody.rotation.eulerAngles.y, 0f) * direction;

            float targetRotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ”чтите поворот персонажа при вычислении угла
            targetRotZ += _rotationOffset + _characterBody.rotation.eulerAngles.z;

            if (targetRotZ < 0f)
            {
                targetRotZ *= -1;
            }

            float clampedRotZ = Mathf.Clamp(targetRotZ, _minRotation, _maxRotation);

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, clampedRotZ);
            Quaternion bodyRotation = Quaternion.Euler(0f, _characterBody.rotation.eulerAngles.y, 0f);
            Quaternion finalRotation = bodyRotation * targetRotation;

            _handWithPistol.rotation = Quaternion.Slerp(_handWithPistol.rotation, finalRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}