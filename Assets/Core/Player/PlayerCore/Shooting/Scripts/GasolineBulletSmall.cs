using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using PlayerSpace;

namespace BulletsSpace
{
    public class GasolineBulletSmall : MonoBehaviour
    {
        private Tilemap _groundTilemap;
        private Tilemap _puddlesUpTilemap;
        private Tilemap _puddlesDownTilemap;
        private Tilemap _puddlesRightTilemap;
        private Tilemap _puddlesLeftTilemap;

        [SerializeField] private TileBase _puddleTileUp;
        [SerializeField] private TileBase _puddleTileDown;
        [SerializeField] private TileBase _puddleTileRight;
        [SerializeField] private TileBase _puddleTileLeft;

        [SerializeField] private TileBase[] _groundGroupNotPuddle = new TileBase[3];
        [SerializeField] private TileBase[] _groundGroupBurned = new TileBase[3];

        private void OnEnable()
        {
            UpdateTilemaps();
        }

        private void UpdateTilemaps()
        {
            _groundTilemap = GameObject.FindGameObjectWithTag("GroundGrid").GetComponent<Tilemap>();
            _puddlesUpTilemap = GameObject.FindGameObjectWithTag("PuddlesUpGrid").GetComponent<Tilemap>();
            _puddlesDownTilemap = GameObject.FindGameObjectWithTag("PuddlesDownGrid").GetComponent<Tilemap>();
            _puddlesRightTilemap = GameObject.FindGameObjectWithTag("PuddlesRightGrid").GetComponent<Tilemap>();
            _puddlesLeftTilemap = GameObject.FindGameObjectWithTag("PuddlesLeftGrid").GetComponent<Tilemap>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("GroundGrid"))
            {
                ContactPoint2D contact = collision.contacts[0];
                Vector3 point = contact.point;

                Vector3Int nearestTilePosition = FindNearestTile(point, 1f); // ���������� ������ 1f ��� ������ ���������� �����

                if (nearestTilePosition != Vector3Int.zero)
                {
                    Vector2 collisionNormal = contact.normal;

                    // �������� ����������� ��������� � ��������� ���������������� ����� �� _puddlesTilemap
                    if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
                    {
                        if (collisionNormal.x > 0)
                        {
                            if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(nearestTilePosition)))
                            {
                                if (_puddlesRightTilemap.GetTile(nearestTilePosition) == null || _groundGroupBurned.Contains(_puddlesRightTilemap.GetTile(nearestTilePosition)))
                                {
                                    _puddlesRightTilemap.SetTile(nearestTilePosition, _puddleTileRight);
                                }
                            }
                        }
                        else
                        {
                            if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(nearestTilePosition)))
                            {
                                if (_puddlesLeftTilemap.GetTile(nearestTilePosition) == null || _groundGroupBurned.Contains(_puddlesLeftTilemap.GetTile(nearestTilePosition)))
                                {
                                    _puddlesLeftTilemap.SetTile(nearestTilePosition, _puddleTileLeft);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (collisionNormal.y > 0)
                        {
                            if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(nearestTilePosition)))
                            {
                                if (_puddlesUpTilemap.GetTile(nearestTilePosition) == null || _groundGroupBurned.Contains(_puddlesUpTilemap.GetTile(nearestTilePosition)))
                                {
                                    _puddlesUpTilemap.SetTile(nearestTilePosition, _puddleTileUp);
                                }
                            }
                        }
                        else
                        {
                            if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(nearestTilePosition)))
                            {
                                if (_puddlesDownTilemap.GetTile(nearestTilePosition) == null || _groundGroupBurned.Contains(_puddlesDownTilemap.GetTile(nearestTilePosition)))
                                {
                                    _puddlesDownTilemap.SetTile(nearestTilePosition, _puddleTileDown);
                                }
                            }
                        }
                    }
                }
            }

            gameObject.SetActive(false);
        }

        private Vector3Int FindNearestTile(Vector3 point, float radius)
        {
            Vector3Int collisionCellPosition = _groundTilemap.WorldToCell(point);

            // ����������� ������ ������ ������ ������� ���������
            int minX = Mathf.FloorToInt(collisionCellPosition.x - radius);
            int maxX = Mathf.CeilToInt(collisionCellPosition.x + radius);
            int minY = Mathf.FloorToInt(collisionCellPosition.y - radius);
            int maxY = Mathf.CeilToInt(collisionCellPosition.y + radius);

            float minDistance = Mathf.Infinity;
            Vector3Int nearestTilePosition = Vector3Int.zero;

            // ����� ���������� ����� � ��������� �������
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);

                    if (_groundTilemap.HasTile(cellPosition))
                    {
                        Vector3 tileCenter = _groundTilemap.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0f);
                        float distance = Vector3.Distance(point, tileCenter);

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestTilePosition = cellPosition;
                        }
                    }
                }
            }

            return nearestTilePosition;
        }
    }
}