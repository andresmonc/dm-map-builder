using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapInitializer : Singleton<TileMapInitializer>
{
    [SerializeField] List<BuildingCategory> categoriesToCreateTileMapFor;
    [SerializeField] Transform tileMapParent;
    private void Start()
    {
        CreateMaps();
    }

    private void CreateMaps()
    {
        foreach (BuildingCategory category in categoriesToCreateTileMapFor)
        {
            GameObject tileMap = new GameObject(category.TileMapName);
            tileMap.AddComponent<Tilemap>();
            tileMap.AddComponent<TilemapRenderer>();
            tileMap.transform.SetParent(tileMapParent);
        }
    }
}
