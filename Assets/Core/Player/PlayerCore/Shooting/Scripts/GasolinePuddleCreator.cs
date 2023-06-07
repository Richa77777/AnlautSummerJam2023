using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasolinePuddleCreator : MonoBehaviour
{
    private Tilemap tilemap;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float yOffset = 0.46f;

        Vector3 contactPoint = collision.contacts[0].point;
        Vector3Int tilePosition = tilemap.WorldToCell(contactPoint);
        Vector3 puddlePosition = new Vector3(tilemap.GetCellCenterWorld(tilePosition).x, tilemap.GetCellCenterWorld(tilePosition).y - yOffset, 0f);

        GameObject puddle = PoolsController.Instance.GetPuddlesPool.GetObjectFromPool();
        puddle.transform.position = puddlePosition;
        puddle.SetActive(true);

        gameObject.SetActive(false);
    }
}
