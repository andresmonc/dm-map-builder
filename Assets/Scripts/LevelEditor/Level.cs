using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Level : INetworkSerializable
{
    public string name;
    public string saveTime;
    [NonSerialized]
    public List<Tuple<string, Tilemap>> tilemaps = new List<Tuple<string, Tilemap>>();

    public List<TilemapData> data;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        this.PrepareToSave(SaveHandler.tileBaseToBuildingObject);
        serializer.SerializeValue(ref name);
        serializer.SerializeValue(ref saveTime);

        int count = data.Count;
        serializer.SerializeValue(ref count);

        if (serializer.IsReader)
        {
            data.Clear();
            for (int i = 0; i < count; i++)
            {
                TilemapData tilemapData = new TilemapData();
                tilemapData.NetworkSerialize(serializer);
                data.Add(tilemapData);
            }
        }
        else
        {
            foreach (TilemapData tilemapData in data)
            {
                tilemapData.NetworkSerialize(serializer);
            }
        }
    }

    public void PrepareToSave(Dictionary<TileBase, BuildingObjectBase> tileBaseToBuildingObject)
    {
        saveTime = DateTime.Now.ToString("g");
        data = new List<TilemapData>();
        foreach (var mapObj in tilemaps)
        {
            if (mapObj.Item1.Contains("Preview") || mapObj.Item1.Contains("Visible"))
            {
                continue;
            }
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
            data.Add(mapData);
        }
    }
}

[Serializable]
public class TilemapData : INetworkSerializable
{
    public string key; // the key of your dictionary for the tilemap - here: the name of the map in the hierarchy
    public List<TileInfo> tiles = new List<TileInfo>();

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref key);
        int count = tiles.Count;
        serializer.SerializeValue(ref count);
        if (serializer.IsReader)
        {
            tiles.Clear();
            for (int i = 0; i < count; i++)
            {
                TileInfo tile = new TileInfo(Vector3Int.zero, string.Empty);
                tile.NetworkSerialize(serializer);
                tiles.Add(tile);
            }
        }
        else
        {
            foreach (TileInfo tile in tiles)
            {
                tile.NetworkSerialize(serializer);
            }
        }
    }
}

[Serializable]
public class TileInfo : INetworkSerializable
{
    public string guidForBuildable;
    public Vector3Int position;

    public TileInfo(Vector3Int pos, string guid)
    {
        position = pos;
        guidForBuildable = guid;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref guidForBuildable);
        serializer.SerializeValue(ref position);
    }
}