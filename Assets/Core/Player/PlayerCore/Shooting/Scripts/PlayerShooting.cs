using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform _handWithPistol;
        [SerializeField] private Transform _characterBody;
        [SerializeField] private Transform _shootPoint;

        [SerializeField] private float _rotationOffset;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _minRotation = 0f;
        [SerializeField] private float _maxRotation = 150f;

        [SerializeField] private float _smallShotReloadTime = 0.3f;
        [SerializeField] private float _bigShotReloadTime = 1f;

        private bool _smallShotAvailable = true;
        private bool _bigShotAvailable = true;

        public Transform GetHandWithPistol => _handWithPistol;
        public Transform GetShootPoint => _shootPoint;

        private void Update()
        {
            SetRotation();

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
                    BigShot();
                }
            }
        }

        private void SmallShot()
        {
            GameObject bullet = PoolsController.Instance.GetSmallBulletsPool.GetObjectFromPool();
            bullet.transform.position = _shootPoint.position;
            bullet.transform.rotation = _handWithPistol.transform.rotation;
            bullet.SetActive(true);

            StartCoroutine(SmallShotReloading());
        }

        private void BigShot()
        {
            GameObject bullet = PoolsController.Instance.GetBigBulletsPool.GetObjectFromPool();
            print("BigShot");

            StartCoroutine(BigShotReloading());
        }

        private IEnumerator SmallShotReloading()
        {
            yield return new WaitForSeconds(_smallShotReloadTime);
            _smallShotAvailable = true;
        }

        private IEnumerator BigShotReloading()
        {
            yield return new WaitForSeconds(_bigShotReloadTime);
            _bigShotAvailable = true;
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