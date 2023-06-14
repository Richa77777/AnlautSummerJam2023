using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireSpace
{
    public class BurnedTilesController : MonoBehaviour
    {
        public static BurnedTilesController Instance;

        [SerializeField] private float _burnedTilesLifetime = 10f;

        [Space(5f)]

        [SerializeField] private TileBase _puddleTileUpBurned;
        [SerializeField] private TileBase _puddleTileDownBurned;
        [SerializeField] private TileBase _puddleTileRightBurned;
        [SerializeField] private TileBase _puddleTileLeftBurned;

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

        public void BurnedTileLiftCycleStart(Vector3Int tilePos, Tilemap tilemap)
        {
            StartCoroutine(BurnedTileLifeCycle(tilePos, tilemap));
        }

        private IEnumerator BurnedTileLifeCycle(Vector3Int tilePos, Tilemap tilemap)
        {
            yield return new WaitForSeconds(_burnedTilesLifetime);

            if (tilemap.GetTile(tilePos) == _puddleTileUpBurned || tilemap.GetTile(tilePos) == _puddleTileDownBurned || tilemap.GetTile(tilePos) == _puddleTileRightBurned || tilemap.GetTile(tilePos) == _puddleTileLeftBurned)
            {
                tilemap.SetTile(tilePos, null);
            }
        }
    }
}

