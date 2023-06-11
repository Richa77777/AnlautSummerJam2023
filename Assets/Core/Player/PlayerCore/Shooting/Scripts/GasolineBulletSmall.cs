using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasolineBulletSmall : MonoBehaviour
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

        Vector3Int nearestTilePosition = FindNearestTile(point, 1f); // Используем радиус 1f для поиска ближайшего тайла

        if (nearestTilePosition != Vector3Int.zero)
        {
            Vector2 collisionNormal = contact.normal;

            // Проверка направления попадания и установка соответствующего тайла на _puddlesTilemap
            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
            {
                if (collisionNormal.x > 0)
                {
                    if (_immortalPuddlesTilemap.GetTile(nearestTilePosition) != _puddleTileRight)
                    {
                        _puddlesRightTilemap.SetTile(nearestTilePosition, _puddleTileRight);
                    }
                }
                else
                {
                    if (_immortalPuddlesTilemap.GetTile(nearestTilePosition) != _puddleTileLeft)
                    {
                        _puddlesLeftTilemap.SetTile(nearestTilePosition, _puddleTileLeft);
                    }
               }
            }
            else
            {
                if (collisionNormal.y > 0)
                {
                    if (_immortalPuddlesTilemap.GetTile(nearestTilePosition) != _puddleTileUp)
                    {
                        _puddlesUpTilemap.SetTile(nearestTilePosition, _puddleTileUp);
                    }
                }
                else
                {
                    if (_immortalPuddlesTilemap.GetTile(nearestTilePosition) != _puddleTileDown)
                    {
                        _puddlesDownTilemap.SetTile(nearestTilePosition, _puddleTileDown);
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }

    private Vector3Int FindNearestTile(Vector3 point, float radius)
    {
        Vector3Int collisionCellPosition = _groundTilemap.WorldToCell(point);

        // Определение границ поиска вокруг позиции попадания
        int minX = Mathf.FloorToInt(collisionCellPosition.x - radius);
        int maxX = Mathf.CeilToInt(collisionCellPosition.x + radius);
        int minY = Mathf.FloorToInt(collisionCellPosition.y - radius);
        int maxY = Mathf.CeilToInt(collisionCellPosition.y + radius);

        float minDistance = Mathf.Infinity;
        Vector3Int nearestTilePosition = Vector3Int.zero;

        // Поиск ближайшего тайла в указанном радиусе
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, collisionCellPosition.z);

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