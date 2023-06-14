using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireSpace
{
    public class Canister : MonoBehaviour
    {
        [SerializeField] private TileBase _puddleTileUp;
        [SerializeField] private TileBase _puddleTileDown;
        [SerializeField] private TileBase _puddleTileRight;
        [SerializeField] private TileBase _puddleTileLeft;

        [SerializeField] private TileBase _puddleTileUpBurned;
        [SerializeField] private TileBase _puddleTileDownBurned;
        [SerializeField] private TileBase _puddleTileRightBurned;
        [SerializeField] private TileBase _puddleTileLeftBurned;

        private Tilemap _puddlesUpTilemap;
        private Tilemap _puddlesDownTilemap;
        private Tilemap _puddlesRightTilemap;
        private Tilemap _puddlesLeftTilemap;

        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            UpdateTilemaps();
        }

        private void UpdateTilemaps()
        {
            _puddlesUpTilemap = GameObject.FindGameObjectWithTag("PuddlesUpGrid").GetComponent<Tilemap>();
            _puddlesDownTilemap = GameObject.FindGameObjectWithTag("PuddlesDownGrid").GetComponent<Tilemap>();
            _puddlesRightTilemap = GameObject.FindGameObjectWithTag("PuddlesRightGrid").GetComponent<Tilemap>();
            _puddlesLeftTilemap = GameObject.FindGameObjectWithTag("PuddlesLeftGrid").GetComponent<Tilemap>();
        }

        private void Update()
        {
            List<Vector3Int> upPuddles = FindNearestPuddleTile(_puddlesUpTilemap);
            List<Vector3Int> downPuddles = FindNearestPuddleTile(_puddlesDownTilemap);
            List<Vector3Int> rightPuddles = FindNearestPuddleTile(_puddlesRightTilemap);
            List<Vector3Int> leftPuddles = FindNearestPuddleTile(_puddlesLeftTilemap);

            Vector3Int firePos = Vector3Int.zero;

            for (int i = 0; i < upPuddles.Count; i++)
            {
                firePos = upPuddles[i];
                firePos.y += 1;

                if (_puddlesUpTilemap.GetTile(firePos) != _puddleTileUpBurned)
                {
                    FireController.Instance.TryIgniteTile(_puddlesUpTilemap.GetCellCenterWorld(firePos), FireSides.Up);
                }
            }

            for (int i = 0; i < downPuddles.Count; i++)
            {
                firePos = downPuddles[i];
                firePos.y -= 1;

                if (_puddlesDownTilemap.GetTile(firePos) != _puddleTileDownBurned)
                {
                    FireController.Instance.TryIgniteTile(_puddlesDownTilemap.GetCellCenterWorld(firePos), FireSides.Down);
                }
            }

            for (int i = 0; i < rightPuddles.Count; i++)
            {
                firePos = rightPuddles[i];
                firePos.x += 1;

                if (_puddlesRightTilemap.GetTile(firePos) != _puddleTileRightBurned)
                {
                    FireController.Instance.TryIgniteTile(_puddlesRightTilemap.GetCellCenterWorld(firePos), FireSides.Right);
                }
            }

            for (int i = 0; i < leftPuddles.Count; i++)
            {
                firePos = leftPuddles[i];
                firePos.x -= 1;

                if (_puddlesLeftTilemap.GetTile(firePos) != _puddleTileLeftBurned)
                {
                    FireController.Instance.TryIgniteTile(_puddlesLeftTilemap.GetCellCenterWorld(firePos), FireSides.Left);
                }
            }
        }

        private List<Vector3Int> FindNearestPuddleTile(Tilemap tilemap)
        {
            List<Vector3Int> nearestTilesPosition = new List<Vector3Int>();

            Bounds bounds = _collider.bounds;
            Vector3Int minPosition = tilemap.WorldToCell(bounds.min);
            Vector3Int maxPosition = tilemap.WorldToCell(bounds.max);

            for (int x = minPosition.x; x <= maxPosition.x; x++)
            {
                for (int y = minPosition.y; y <= maxPosition.y; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    TileBase tile = tilemap.GetTile(tilePosition);

                    if (tile != null)
                    {
                        if (tilemap.GetTile(tilePosition) == _puddleTileUp || tilemap.GetTile(tilePosition) == _puddleTileDown || tilemap.GetTile(tilePosition) == _puddleTileRight || tilemap.GetTile(tilePosition) == _puddleTileLeft)
                        {
                            nearestTilesPosition.Add(tilePosition);
                        }
                    }
                }
            }

            return nearestTilesPosition;
        }
    }
}