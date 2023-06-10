using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
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

        private void Update()
        {
            SetRotation();

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            GameObject bullet = PoolsController.Instance.GetBigBulletsPool.GetObjectFromPool();
            bullet.transform.position = _shootPoint.position;
            bullet.transform.rotation = _handWithPistol.transform.rotation;
            bullet.SetActive(true);
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