using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSpace
{
    public class PoolsController : MonoBehaviour
    {
        public static PoolsController Instance;

        [SerializeField] private GameObject _smallBulletsPoolPrefab;
        [SerializeField] private GameObject _bigBulletsPoolPrefab;
        [SerializeField] private GameObject _firesUpPoolPrefab;
        [SerializeField] private GameObject _firesDownPoolPrefab;
        [SerializeField] private GameObject _firesRightPoolPrefab;
        [SerializeField] private GameObject _firesLeftPoolPrefab;

        private ObjectsPool _smallBulletsPool;
        private ObjectsPool _bigBulletsPool;
        private ObjectsPool _firesUpPool;
        private ObjectsPool _firesDownPool;
        private ObjectsPool _firesRightPool;
        private ObjectsPool _firesLeftPool;

        public ObjectsPool GetSmallBulletsPool => _smallBulletsPool;
        public ObjectsPool GetBigBulletsPool => _bigBulletsPool;
        public ObjectsPool GetFiresUpPool => _firesUpPool;
        public ObjectsPool GetFiresDownPool => _firesDownPool;
        public ObjectsPool GetFiresRightPool => _firesRightPool;
        public ObjectsPool GetFiresLeftPool => _firesLeftPool;

        private void Awake()
        {
            #region Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            #endregion

            _smallBulletsPool = Instantiate(_smallBulletsPoolPrefab).GetComponent<ObjectsPool>();
            _bigBulletsPool = Instantiate(_bigBulletsPoolPrefab).GetComponent<ObjectsPool>();

            _firesUpPool = Instantiate(_firesUpPoolPrefab).GetComponent<ObjectsPool>();
            _firesDownPool = Instantiate(_firesDownPoolPrefab).GetComponent<ObjectsPool>();
            _firesRightPool = Instantiate(_firesRightPoolPrefab).GetComponent<ObjectsPool>();
            _firesLeftPool = Instantiate(_firesLeftPoolPrefab).GetComponent<ObjectsPool>();

            DontDestroyOnLoad(_smallBulletsPool);
            DontDestroyOnLoad(_bigBulletsPool);
            DontDestroyOnLoad(_firesUpPool);
            DontDestroyOnLoad(_firesDownPool);
            DontDestroyOnLoad(_firesRightPool);
            DontDestroyOnLoad(_firesLeftPool);
        }
    }
}
