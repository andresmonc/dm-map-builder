using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisibleGridDisplay : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase tile;
    [SerializeField] Vector3Int minBounds;
    [SerializeField] Vector3Int maxBounds;


    void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned.");
            return;
        }

        if (tile == null)
        {
            Debug.LogError("Tile is not assigned.");
            return;
        }

        for (int x = minBounds.x; x <= maxBounds.x; x++)
        {
            for (int y = minBounds.y; y <= maxBounds.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                tilemap.SetTile(position, tile);
            }
        }
    }
}


