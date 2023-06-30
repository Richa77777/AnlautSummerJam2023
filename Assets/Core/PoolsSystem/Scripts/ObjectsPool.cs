using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSpace
{
    public class ObjectsPool : MonoBehaviour
    {
        [SerializeField] private int _poolLength;
        [SerializeField] private GameObject _objectPrefab;

        private List<GameObject> _objectsPool = new List<GameObject>();

        public List<GameObject> ObjectsPoolGet => new List<GameObject>(_objectsPool);

        private void Awake()
        {
            GameObject obj;

            for (int i = 0; i < _poolLength; i++)
            {
                obj = Instantiate(_objectPrefab, transform, true);
                obj.SetActive(false);

                _objectsPool.Add(obj);
            }
        }

        public GameObject GetObjectFromPool()
        {
            GameObject freeObject = null;

            for (int i = 0; i < _objectsPool.Count; i++)
            {
                if (_objectsPool[i].activeInHierarchy == false)
                {
                    freeObject = _objectsPool[i];
                    break;
                }
            }

            return freeObject;
        }
    }
}
