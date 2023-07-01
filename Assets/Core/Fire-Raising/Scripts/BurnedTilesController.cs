using PlayerSpace;
using System.Linq;
using System.Collections;
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

        [SerializeField] private TileBase _puddleTileUp;
        [SerializeField] private TileBase _puddleTileDown;
        [SerializeField] private TileBase _puddleTileRight;
        [SerializeField] private TileBase _puddleTileLeft;

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
            bool might = true;
            TileBase tile = tilemap.GetTile(tilePos);

            for (float t = 0; t < _burnedTilesLifetime; t += Time.deltaTime)
            {
                if (tilemap != null)
                {
                    if (tilemap.GetTile(tilePos) != tile)
                    {
                        might = false;
                        break;
                    }
                }

                yield return null;
            }

            if (tilemap != null && might == true)
            {
                if (tilemap.GetTile(tilePos) == _puddleTileUpBurned || tilemap.GetTile(tilePos) == _puddleTileDownBurned || tilemap.GetTile(tilePos) == _puddleTileRightBurned || tilemap.GetTile(tilePos) == _puddleTileLeftBurned)
                {
                    tilemap.SetTile(tilePos, null);
                }
            }
        }
    }
}

