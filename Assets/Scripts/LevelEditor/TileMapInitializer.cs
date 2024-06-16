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
            GameObject tileMapGameObject = new GameObject(category.TileMapName);
            Tilemap tileMapComponent = tileMapGameObject.AddComponent<Tilemap>();
            TilemapRenderer tileMapRenderer = tileMapGameObject.AddComponent<TilemapRenderer>();
            tileMapGameObject.transform.SetParent(tileMapParent);
            category.Tilemap = tileMapComponent;
            tileMapRenderer.sortingOrder = category.SortingOrder;
        }
    }
}
