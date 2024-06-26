using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Level
{
    [NonSerialized]
    public List<Tuple<string, Tilemap>> tilemaps = new List<Tuple<string, Tilemap>>();
    public DateTime LastModified { get; private set; }

    public List<TilemapData> Data { get; private set; }

    public void PrepareToSave(Dictionary<TileBase, BuildingObjectBase> tileBaseToBuildingObject)
    {
        Data = new List<TilemapData>();
        foreach (var mapObj in tilemaps)
        {
            TilemapData mapData = new TilemapData();
            mapData.key = mapObj.Item1;
            BoundsInt boundsForThisMap = mapObj.Item2.cellBounds;
            for (int x = boundsForThisMap.xMin; x < boundsForThisMap.xMax; x++)
            {
                for (int y = boundsForThisMap.yMin; y < boundsForThisMap.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tile = mapObj.Item2.GetTile(pos);

                    if (tile != null && tileBaseToBuildingObject.ContainsKey(tile))
                    {
                        string guid = tileBaseToBuildingObject[tile].name;
                        TileInfo ti = new TileInfo(pos, guid);
                        // Add "TileInfo" to "Tiles" List of "TilemapData"
                        mapData.tiles.Add(ti);
                    }
                }
            }
            // Add "TilemapData" Object to List
            Data.Add(mapData);
        }
    }

}
