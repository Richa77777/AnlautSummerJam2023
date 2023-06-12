using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSpace;

namespace FireSpace
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

        private void IgniteTile(Vector3 firePos, FireSides fireSide)
        {
            GameObject firePrefab = null;
            Vector3 newFirePos = firePos;

            switch (fireSide)
            {
                case FireSides.Up:
                    firePrefab = PoolsController.Instance.GetFiresUpPool.GetObjectFromPool();
                    newFirePos.y -= 1;
                    break;

                case FireSides.Down:
                    firePrefab = PoolsController.Instance.GetFiresDownPool.GetObjectFromPool();
                    newFirePos.y += 1;
                    break;

                case FireSides.Right:
                    firePrefab = PoolsController.Instance.GetFiresRightPool.GetObjectFromPool();
                    newFirePos.x -= 1;
                    break;

                case FireSides.Left:
                    firePrefab = PoolsController.Instance.GetFiresLeftPool.GetObjectFromPool();
                    newFirePos.x += 1;
                    break;
            }

            firePrefab.transform.position = firePos;
            firePrefab.SetActive(true);
            firePrefab.GetComponent<Fire>().FirePos = newFirePos;

            _cellsWithFire.Add(newFirePos);
        }

        public void TryIgniteTile(Vector3 firePos, FireSides fireSide)
        {
            if (fireSide == FireSides.Up)
            {
                if (!_cellsWithFire.Contains(new Vector3(firePos.x, firePos.y - 1)))
                {
                    IgniteTile(firePos, fireSide);
                }
            }

            else if (fireSide == FireSides.Down)
            {
                if (!_cellsWithFire.Contains(new Vector3(firePos.x, firePos.y + 1)))
                {
                    IgniteTile(firePos, fireSide);
                }
            }

            else if (fireSide == FireSides.Right)
            {
                if (!_cellsWithFire.Contains(new Vector3(firePos.x - 1, firePos.y)))
                {
                    IgniteTile(firePos, fireSide);
                }
            }

            else if (fireSide == FireSides.Left)
            {
                if (!_cellsWithFire.Contains(new Vector3(firePos.x + 1, firePos.y)))
                {
                    IgniteTile(firePos, fireSide);
                }
            }
        }

        public void FiringEnd(Vector3 firePos)
        {
            _cellsWithFire.Remove(firePos);
        }
    }
}
