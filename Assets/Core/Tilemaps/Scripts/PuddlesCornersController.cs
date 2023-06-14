using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PuddlesCornersController : MonoBehaviour
{
    public static PuddlesCornersController Instance;

    [SerializeField] private TileBase _puddleCornerTileTopRight;
    [SerializeField] private TileBase _puddleCornerTileBottomRight;
    [SerializeField] private TileBase _puddleCornerTileTopLeft;
    [SerializeField] private TileBase _puddleCornerTileBottomLeft;


    private Tilemap _puddlesCornersTilemap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _puddlesCornersTilemap = GameObject.FindGameObjectWithTag("PuddlesCornersGrid").GetComponent<Tilemap>();
    }

    public void SetCorner(Vector3Int pos, PuddlesCornersSides side)
    {
        switch (side)
        {
            case PuddlesCornersSides.TopRight:
                _puddlesCornersTilemap.SetTile(pos, _puddleCornerTileTopRight);
                break;

            case PuddlesCornersSides.TopLeft:
                _puddlesCornersTilemap.SetTile(pos, _puddleCornerTileTopLeft);
                break;

            case PuddlesCornersSides.BottomRight:
                _puddlesCornersTilemap.SetTile(pos, _puddleCornerTileBottomRight);
                break;

            case PuddlesCornersSides.BottomLeft:
                _puddlesCornersTilemap.SetTile(pos, _puddleCornerTileBottomLeft);
                break;
        }
    }
}

public enum PuddlesCornersSides
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft
}