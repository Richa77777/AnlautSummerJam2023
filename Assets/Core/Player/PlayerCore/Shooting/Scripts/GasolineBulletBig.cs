using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasolineBulletBig : MonoBehaviour
{
    private Tilemap _groundTilemap;
    private Tilemap _puddlesTilemap;

    [SerializeField] private TileBase _puddleTileUp;
    [SerializeField] private TileBase _puddleTileDown;
    [SerializeField] private TileBase _puddleTileRight;
    [SerializeField] private TileBase _puddleTileLeft;

    private void Start()
    {
        _groundTilemap = GameObject.FindGameObjectWithTag("GroundGrid").GetComponent<Tilemap>();
        _puddlesTilemap = GameObject.FindGameObjectWithTag("PuddlesGrid").GetComponent<Tilemap>();
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
        Vector3Int collisionCellPosition = _groundTilemap.WorldToCell(point);

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
            _puddlesTilemap.SetTile(centerTilePosition, puddleTile);

            TileBase centerGroundTile = _groundTilemap.GetTile(centerTilePosition);

            // Set tiles along the line in each direction if they are available and have the same TileBase as the center tile
            for (int i = 1; i <= lineLength; i++)
            {
                Vector3Int leftTilePosition = centerTilePosition + new Vector3Int(-i, 0, 0);
                Vector3Int rightTilePosition = centerTilePosition + new Vector3Int(i, 0, 0);
                if (_groundTilemap.HasTile(leftTilePosition) && _groundTilemap.GetTile(leftTilePosition) == centerGroundTile)
                {
                    _puddlesTilemap.SetTile(leftTilePosition, puddleTile);
                }
                if (_groundTilemap.HasTile(rightTilePosition) && _groundTilemap.GetTile(rightTilePosition) == centerGroundTile)
                {
                    _puddlesTilemap.SetTile(rightTilePosition, puddleTile);
                }

                Vector3Int downTilePosition = centerTilePosition + new Vector3Int(0, -i, 0);
                Vector3Int upTilePosition = centerTilePosition + new Vector3Int(0, i, 0);
                if (_groundTilemap.HasTile(downTilePosition) && _groundTilemap.GetTile(downTilePosition) == centerGroundTile)
                {
                    _puddlesTilemap.SetTile(downTilePosition, puddleTile);
                }
                if (_groundTilemap.HasTile(upTilePosition) && _groundTilemap.GetTile(upTilePosition) == centerGroundTile)
                {
                    _puddlesTilemap.SetTile(upTilePosition, puddleTile);
                }
            }
        }
    }
}
