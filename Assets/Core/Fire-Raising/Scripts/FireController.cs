using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireRaising
{
    public class FireController : MonoBehaviour
    {
        public static FireController Instance;

        [SerializeField] private List<Vector3> _cellsWithFire = new List<Vector3>();

        public List<Vector3> GetCellsWithFireList
        {
            get
            {
                return new List<Vector3>(_cellsWithFire);
            }
        }

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
        }

        public void IgniteTile(Vector3 firePos, FireSides fireSide)
        {
            GameObject firePrefab = null;

            switch (fireSide)
            {
                case FireSides.Up:
                    firePrefab = PoolsController.Instance.GetFiresUpPool.GetObjectFromPool();
                    break;

                case FireSides.Down:
                    firePrefab = PoolsController.Instance.GetFiresDownPool.GetObjectFromPool();
                    break;

                case FireSides.Right:
                    firePrefab = PoolsController.Instance.GetFiresRightPool.GetObjectFromPool();
                    break;

                case FireSides.Left:
                    firePrefab = PoolsController.Instance.GetFiresLeftPool.GetObjectFromPool();
                    break;
            }

            firePrefab.transform.position = firePos;
            firePrefab.SetActive(true);
            firePrefab.GetComponent<Fire>().FirePos = firePos;

            _cellsWithFire.Add(firePos);
        }

        public void FiringEnd(Vector3 firePos)
        {
            _cellsWithFire.Remove(firePos);
        }
    }
}
