using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        private PlayerShooting _playerShootingComponent;
        private PlayerMovement _playerMovementComponent;
        private PlayerDie _playerDieComponent;

        public PlayerShooting GetPlayerShootingComponent => _playerShootingComponent;
        public PlayerMovement GetPlayerMovementComponent => _playerMovementComponent;
        public PlayerDie GetPlayerDieComponent => _playerDieComponent;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);;
            }
            else
            {
                Destroy(gameObject);
            }

            _playerShootingComponent = GetComponent<PlayerShooting>();
            _playerMovementComponent = GetComponent<PlayerMovement>();
            _playerDieComponent = GetComponent<PlayerDie>();
        }
    }
}
