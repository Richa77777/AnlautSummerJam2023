using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasolinePuddleCreator : MonoBehaviour
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
        Vector2 collisionNormal = contact.normal;
        Vector3 point = contact.point;
        Vector3Int collisionCellPosition = _groundTilemap.WorldToCell(point);

        if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
        {
            if (collisionNormal.x > 0)
            {
                collisionCellPosition = new Vector3Int(collisionCellPosition.x, collisionCellPosition.y);
                _puddlesTilemap.SetTile(collisionCellPosition, _puddleTileRight);
                print("Справа");
            }
            else
            {
                collisionCellPosition = new Vector3Int(collisionCellPosition.x, collisionCellPosition.y);
                _puddlesTilemap.SetTile(collisionCellPosition, _puddleTileLeft);
                print("Слева");
            }
        }
        else
        {
            if (collisionNormal.y > 0)
            {
                collisionCellPosition = new Vector3Int(collisionCellPosition.x, collisionCellPosition.y - 1);
                _puddlesTilemap.SetTile(collisionCellPosition, _puddleTileUp);
                print("Сверху");
            }
            else
            {
                collisionCellPosition = new Vector3Int(collisionCellPosition.x, collisionCellPosition.y);
                _puddlesTilemap.SetTile(collisionCellPosition, _puddleTileDown);
                print("Снизу");
            }
        }

        gameObject.SetActive(false);
    }
}
