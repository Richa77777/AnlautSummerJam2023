using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        private PlayerShooting _playerShootingComponent;

        public PlayerShooting GetPlayerShootingComponent => _playerShootingComponent;

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
        }
    }
}
