using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PlayerSpace;

namespace BulletsSpace
{
    public class GasolineBulletBig : MonoBehaviour
    {
        private Tilemap _groundTilemap;

        private Tilemap _puddlesUpTilemap;

        [SerializeField] private TileBase _puddleTileUp;
        [SerializeField] private TileBase _puddleTileUpBurned;

        [SerializeField] private TileBase[] _groundGroupHorizontal = new TileBase[3];
        [SerializeField] private TileBase[] _groundGroupNotPuddle = new TileBase[3];

        private void OnEnable()
        {
            UpdateTilemaps();
        }

        private void UpdateTilemaps()
        {
            _groundTilemap = GameObject.FindGameObjectWithTag("GroundGrid").GetComponent<Tilemap>();
            _puddlesUpTilemap = GameObject.FindGameObjectWithTag("PuddlesUpGrid").GetComponent<Tilemap>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector3 point = contact.point;

            Vector3Int nearestTilePosition = FindNearestTile(point); // Use the line length instead of radius

            if (nearestTilePosition != Vector3Int.zero && _groundTilemap.HasTile(nearestTilePosition))
            {
                Vector2 collisionNormal = contact.normal;

                // Check the direction of impact and set the corresponding tile on _puddlesTilemap
                if (Mathf.Abs(collisionNormal.x) < Mathf.Abs(collisionNormal.y))
                {
                    if (collisionNormal.y > 0)
                    {
                        SetPuddleTiles(nearestTilePosition, _puddleTileUp, 3);
                    }
                }
            }

            gameObject.SetActive(false);
        }

        private Vector3Int FindNearestTile(Vector3 point)
        {
            float minDistance = Mathf.Infinity;
            Vector3Int nearestTilePosition = Vector3Int.zero;

            // Search for the nearest tile
            foreach (Vector3Int cellPosition in _groundTilemap.cellBounds.allPositionsWithin)
            {
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

            return nearestTilePosition;
        }

        private void SetPuddleTiles(Vector3Int centerTilePosition, TileBase puddleTile, int lineLength)
        {
            if (_groundTilemap.HasTile(centerTilePosition))
            {
                Tilemap currentTilemap = null;

                if (puddleTile == _puddleTileUp)
                {
                    currentTilemap = _puddlesUpTilemap;
                }

                if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(centerTilePosition)))
                {
                    if (currentTilemap.GetTile(centerTilePosition) == null || currentTilemap.GetTile(centerTilePosition) == _puddleTileUpBurned)
                    {
                        currentTilemap.SetTile(centerTilePosition, puddleTile);
                    }
                }

                TileBase centerGroundTile = _groundTilemap.GetTile(centerTilePosition);
                TileBase[] tilesGroup = null;

                if (_groundGroupHorizontal.Contains(centerGroundTile))
                {
                    tilesGroup = _groundGroupHorizontal;
                }


                if (tilesGroup != null)
                {
                    bool left = true;
                    bool right = true;

                    // Set tiles along the line in each direction if they are available and have the same TileBase as the center tile
                    for (int i = 1; i <= lineLength; i++)
                    {
                        Vector3Int leftTilePosition = centerTilePosition + new Vector3Int(-i, 0, 0);
                        Vector3Int rightTilePosition = centerTilePosition + new Vector3Int(i, 0, 0);

                        if (puddleTile == _puddleTileUp)
                        {
                            if (_groundTilemap.HasTile(new Vector3Int(leftTilePosition.x, leftTilePosition.y + 1)))
                            {
                                left = false;
                            }

                            if (_groundTilemap.HasTile(new Vector3Int(rightTilePosition.x, rightTilePosition.y + 1)))
                            {
                                right = false;
                            }
                        }


                        if (_groundTilemap.HasTile(rightTilePosition) && tilesGroup.Contains(_groundTilemap.GetTile(rightTilePosition)) && right == true)
                        {
                            if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(rightTilePosition)))
                            {
                                if (currentTilemap.GetTile(rightTilePosition) == null || currentTilemap.GetTile(rightTilePosition) == _puddleTileUpBurned)
                                {
                                    currentTilemap.SetTile(rightTilePosition, puddleTile);
                                }
                            }
                        }

                        if (_groundTilemap.HasTile(leftTilePosition) && tilesGroup.Contains(_groundTilemap.GetTile(leftTilePosition)) && left == true)
                        {
                            if (!_groundGroupNotPuddle.Contains(_groundTilemap.GetTile(leftTilePosition)))
                            {
                                if (currentTilemap.GetTile(leftTilePosition) == null || currentTilemap.GetTile(leftTilePosition) == _puddleTileUpBurned)
                                {
                                    currentTilemap.SetTile(leftTilePosition, puddleTile);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
