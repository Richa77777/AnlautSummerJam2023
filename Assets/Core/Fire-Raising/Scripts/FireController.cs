using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSpace;
using UnityEngine.Tilemaps;
using PlayerSpace;

namespace FireSpace
{
    public class FireController : MonoBehaviour
    {
        public static FireController Instance;

        [SerializeField] private List<TwoValueContainer<Vector3, FireSides>> _cellsWithFire = new List<TwoValueContainer<Vector3, FireSides>>();
        [SerializeField] private AudioClip _firingSound;

        private AudioSource _audioSource;

        public List<TwoValueContainer<Vector3, FireSides>> GetCellsWithFireList
        {
            get
            {
                return new List<TwoValueContainer<Vector3, FireSides>>(_cellsWithFire);
            }
        }

        public void ClearList()
        {
            _cellsWithFire.Clear();
        }

        private void Update()
        {
            if (_cellsWithFire.Count <= 0)
            {
                _audioSource.Stop();
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

            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.clip = _firingSound;
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
            _cellsWithFire.Add(new TwoValueContainer<Vector3, FireSides>(newFirePos, fireSide));

            firePrefab.transform.position = firePos;
            firePrefab.SetActive(true);
            firePrefab.GetComponent<Fire>().FirePos = newFirePos;

            if (_cellsWithFire.Count > 0)
            {
                _audioSource.Play();
            }
        }

        public void TryIgniteTile(Vector3 firePos, FireSides fireSide)
        {
            if (fireSide == FireSides.Up)
            {
                bool alreadyExists = false;
                foreach (var container in _cellsWithFire)
                {
                    if (container.Value1 == new Vector3(firePos.x, firePos.y - 1) && container.Value2 == fireSide)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    IgniteTile(firePos, fireSide);
                }
            }

            else if (fireSide == FireSides.Down)
            {
                bool alreadyExists = false;
                foreach (var container in _cellsWithFire)
                {
                    if (container.Value1 == new Vector3(firePos.x, firePos.y + 1) && container.Value2 == fireSide)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    IgniteTile(firePos, fireSide);
                }
            }

            else if (fireSide == FireSides.Right)
            {
                bool alreadyExists = false;
                foreach (var container in _cellsWithFire)
                {
                    if (container.Value1 == new Vector3(firePos.x - 1, firePos.y) && container.Value2 == fireSide)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    IgniteTile(firePos, fireSide);
                }
            }

            else if (fireSide == FireSides.Left)
            {
                bool alreadyExists = false;
                foreach (var container in _cellsWithFire)
                {
                    if (container.Value1 == new Vector3(firePos.x + 1, firePos.y) && container.Value2 == fireSide)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    IgniteTile(firePos, fireSide);
                }
            }
        }

        public void FiringEnd(Vector3 firePos, FireSides fireSide)
        {
            foreach (var container in _cellsWithFire)
            {
                if (container.Value1 == firePos && container.Value2 == fireSide)
                {
                    _cellsWithFire.Remove(container);
                    break;
                }
            }

            if (_cellsWithFire.Count <= 0)
            {
                _audioSource.Stop();
            }
        }
    }

    [System.Serializable]
    public class TwoValueContainer<T1, T2>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }

        public TwoValueContainer(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}
