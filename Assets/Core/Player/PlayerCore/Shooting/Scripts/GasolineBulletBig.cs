using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasolineBulletBig : MonoBehaviour
{
    private Tilemap _groundTilemap;

    private Tilemap _puddlesUpTilemap;
    private Tilemap _puddlesDownTilemap;
    private Tilemap _puddlesRightTilemap;
    private Tilemap _puddlesLeftTilemap;
    private Tilemap _immortalPuddlesTilemap;

    [SerializeField] private TileBase _puddleTileUp;
    [SerializeField] private TileBase _puddleTileDown;
    [SerializeField] private TileBase _puddleTileRight;
    [SerializeField] private TileBase _puddleTileLeft;

    [SerializeField] private TileBase[] _groundGroupVertical = new TileBase[3];
    [SerializeField] private TileBase[] _groundGroupHorizontal = new TileBase[3];
    [SerializeField] private TileBase[] _groundGroupAllowVerticalRight = new TileBase[3];
    [SerializeField] private TileBase[] _groundGroupAllowVerticalLeft = new TileBase[3];

    private void Start()
    {
        _groundTilemap = GameObject.FindGameObjectWithTag("GroundGrid").GetComponent<Tilemap>();
        _puddlesUpTilemap = GameObject.FindGameObjectWithTag("PuddlesUpGrid").GetComponent<Tilemap>();
        _puddlesDownTilemap = GameObject.FindGameObjectWithTag("PuddlesDownGrid").GetComponent<Tilemap>();
        _puddlesRightTilemap = GameObject.FindGameObjectWithTag("PuddlesRightGrid").GetComponent<Tilemap>();
        _puddlesLeftTilemap = GameObject.FindGameObjectWithTag("PuddlesLeftGrid").GetComponent<Tilemap>();
        _immortalPuddlesTilemap = GameObject.FindGameObjectWithTag("ImmortalPuddlesGrid").GetComponent<Tilemap>();
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
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
            {
                if (collisionNormal.x > 0)
                {
                    SetPuddleTiles(nearestTilePosition, _puddleTileRight, 3); // Set the line length here (e.g., 5)
                }
                else
                {
                    SetPuddleTiles(nearestTilePosition, _puddleTileLeft, 3);
                }
            }
            else
            {
                if (collisionNormal.y > 0)
                {
                    SetPuddleTiles(nearestTilePosition, _puddleTileUp, 3);
                }
                else
                {
                    SetPuddleTiles(nearestTilePosition, _puddleTileDown, 3);
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

            else if (puddleTile == _puddleTileDown)
            {
                currentTilemap = _puddlesDownTilemap;
            }

            else if (puddleTile == _puddleTileRight)
            {
                currentTilemap = _puddlesRightTilemap;
            }

            else if (puddleTile == _puddleTileLeft)
            {
                currentTilemap = _puddlesLeftTilemap;
            }

            if (_immortalPuddlesTilemap.GetTile(centerTilePosition) != puddleTile)
            {
                currentTilemap.SetTile(centerTilePosition, puddleTile);
            }

            TileBase centerGroundTile = _groundTilemap.GetTile(centerTilePosition);
            TileBase[] tilesGroup = null;

            if (_groundGroupHorizontal.Contains(centerGroundTile))
            {
                tilesGroup = _groundGroupHorizontal;
            }

            else if (_groundGroupVertical.Contains(centerGroundTile))
            {
                tilesGroup = _groundGroupVertical;
            }

            if (tilesGroup != null)
            {
                bool left = true;
                bool right = true;
                bool up = true;
                bool down = true;

                // Set tiles along the line in each direction if they are available and have the same TileBase as the center tile
                for (int i = 1; i <= lineLength; i++)
                {
                    Vector3Int leftTilePosition = centerTilePosition + new Vector3Int(-i, 0, 0);
                    Vector3Int rightTilePosition = centerTilePosition + new Vector3Int(i, 0, 0);
                    Vector3Int downTilePosition = centerTilePosition + new Vector3Int(0, -i, 0);
                    Vector3Int upTilePosition = centerTilePosition + new Vector3Int(0, i, 0);

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

                    if (puddleTile == _puddleTileDown)
                    {
                        if (_groundTilemap.HasTile(new Vector3Int(leftTilePosition.x, leftTilePosition.y - 1)))
                        {
                            left = false;
                        }

                        if (_groundTilemap.HasTile(new Vector3Int(rightTilePosition.x, rightTilePosition.y - 1)))
                        {
                            right = false;
                        }
                    }

                    if (puddleTile == _puddleTileRight)
                    {
                        if (_groundTilemap.HasTile(new Vector3Int(upTilePosition.x + 1, upTilePosition.y)))
                        {
                            up = false;
                        }

                        if (_groundTilemap.HasTile(new Vector3Int(downTilePosition.x + 1, downTilePosition.y)))
                        {
                            down = false;
                        }
                    }

                    if (puddleTile == _puddleTileLeft)
                    {
                        if (_groundTilemap.HasTile(new Vector3Int(upTilePosition.x - 1, upTilePosition.y)))
                        {
                            up = false;
                        }

                        if (_groundTilemap.HasTile(new Vector3Int(downTilePosition.x - 1, downTilePosition.y)))
                        {
                            down = false;
                        }
                    }

                    if (_groundTilemap.HasTile(rightTilePosition) && tilesGroup.Contains(_groundTilemap.GetTile(rightTilePosition)) && right == true)
                    {
                        if (!_immortalPuddlesTilemap.HasTile(rightTilePosition))
                        {
                            if (puddleTile == _puddleTileLeft || puddleTile == _puddleTileRight)
                            {
                                if (puddleTile == _puddleTileRight)
                                {
                                    if (_groundGroupAllowVerticalRight.Contains(_groundTilemap.GetTile(rightTilePosition)))
                                    {
                                        currentTilemap.SetTile(rightTilePosition, puddleTile);
                                    }
                                }

                                else if (_puddlesLeftTilemap == _puddleTileLeft)
                                {
                                    if (_groundGroupAllowVerticalLeft.Contains(_groundTilemap.GetTile(rightTilePosition)))
                                    {
                                        currentTilemap.SetTile(rightTilePosition, puddleTile);
                                    }
                                }

                                return;
                            }

                            currentTilemap.SetTile(rightTilePosition, puddleTile);
                        }
                    }

                    if (_groundTilemap.HasTile(leftTilePosition) && tilesGroup.Contains(_groundTilemap.GetTile(leftTilePosition)) && left == true)
                    {
                        if (!_immortalPuddlesTilemap.HasTile(leftTilePosition))
                        {
                            if (puddleTile == _puddleTileLeft || puddleTile == _puddleTileRight)
                            {
                                if (puddleTile == _puddleTileRight)
                                {
                                    if (_groundGroupAllowVerticalRight.Contains(_groundTilemap.GetTile(leftTilePosition)))
                                    {
                                        currentTilemap.SetTile(leftTilePosition, puddleTile);
                                    }
                                }

                                else if (_puddlesLeftTilemap == _puddleTileLeft)
                                {
                                    if (_groundGroupAllowVerticalLeft.Contains(_groundTilemap.GetTile(leftTilePosition)))
                                    {
                                        currentTilemap.SetTile(leftTilePosition, puddleTile);
                                    }
                                }

                                return;
                            }

                            currentTilemap.SetTile(leftTilePosition, puddleTile);
                        }
                    }

                    if (_groundTilemap.HasTile(upTilePosition) && tilesGroup.Contains(_groundTilemap.GetTile(upTilePosition)) && up == true)
                    {
                        if (!_immortalPuddlesTilemap.HasTile(upTilePosition))
                        {
                            currentTilemap.SetTile(upTilePosition, puddleTile);
                        }
                    }

                    if (_groundTilemap.HasTile(downTilePosition) && tilesGroup.Contains(_groundTilemap.GetTile(downTilePosition)) && down == true)
                    {
                        if (!_immortalPuddlesTilemap.HasTile(downTilePosition))
                        {
                            currentTilemap.SetTile(downTilePosition, puddleTile);
                        }
                    }
                }
            }
        }
    }
}
