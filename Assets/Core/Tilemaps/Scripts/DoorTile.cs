using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorTile : MonoBehaviour
{
    [SerializeField] private TileBase _tile;

    private Tilemap _groundTilemap;

    private void Awake()
    {
        _groundTilemap = GameObject.FindGameObjectWithTag("GroundGrid").GetComponent<Tilemap>();
    }

    private void OnEnable()
    {
        EnableTile();
    }

    private void OnDisable()
    {
        DisableTile();
    }

    public void EnableTile()
    {
        if (_groundTilemap != null)
        {
            _groundTilemap.SetTile(Vector3Int.FloorToInt(new Vector3(transform.position.x, transform.position.y - 1)), _tile);
        }
    }

    public void DisableTile()
    {
        if (_groundTilemap != null)
        {
            _groundTilemap.SetTile(Vector3Int.FloorToInt(new Vector3(transform.position.x, transform.position.y - 1)), null);
        }
    }
}
