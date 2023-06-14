using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlayerSpace
{
    public class PlayerPuddlesController : MonoBehaviour
    {
        public static PlayerPuddlesController Instance;

        [SerializeField] private Dictionary<Vector3Int, Tilemap> _playerPuddles = new Dictionary<Vector3Int, Tilemap>();

        public Dictionary<Vector3Int, Tilemap> GetPlayerPuddles
        {
            get
            {
                return new Dictionary<Vector3Int, Tilemap>(_playerPuddles);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            else
            {
                Destroy(gameObject);
            }
        }

        public void AddPuddle(Vector3Int puddlePos, Tilemap tilemap)
        {
            if (!_playerPuddles.ContainsKey(puddlePos))
            {
                _playerPuddles.Add(puddlePos, tilemap);
            }
        }
    }
}