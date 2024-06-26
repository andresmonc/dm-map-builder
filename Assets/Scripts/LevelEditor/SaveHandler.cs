using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveHandler : MonoBehaviour
{
    Dictionary<TileBase, BuildingObjectBase> tileBaseToBuildingObject = new Dictionary<TileBase, BuildingObjectBase>();
    Dictionary<string, TileBase> guidToTileBase = new Dictionary<string, TileBase>();
    [SerializeField] string fileName = "tilemapData.json";

    private void Start()
    {
        InitTileReferences();
    }

    private void InitTileReferences()
    {
        BuildingObjectBase[] buildables = Resources.LoadAll<BuildingObjectBase>("Scriptables/Buildables");
        foreach (BuildingObjectBase buildable in buildables)
        {
            if (!tileBaseToBuildingObject.ContainsKey(buildable.TileBase))
            {
                tileBaseToBuildingObject.Add(buildable.TileBase, buildable);
                guidToTileBase.Add(buildable.name, buildable.TileBase);
            }
            else
            {
                Debug.LogError("TileBase " + buildable.TileBase.name + " is already in use by " + tileBaseToBuildingObject[buildable.TileBase].name);
            }
        }
    }

    public void OnSave()
    {
        Campaign campaign = CampaignManager.GetInstance().ActiveCampaign;
        foreach (Level level in campaign.levels)
        {
            level.PrepareToSave(tileBaseToBuildingObject);
        }
        FileHandler.SaveToJSON(campaign, fileName);
    }

    public void OnLoad()
    {
        Level level = LevelManager.GetInstance().GetActiveLevel();
        Level data = FileHandler.ReadFromJSON<Campaign>(fileName).levels[0];
        // For Faster lookups convert serializable List<Tuple>> to a dictionary 
        Dictionary<string, Tilemap> levelTileMapsDictionary = level.tilemaps.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

        foreach (var mapData in data.data)
        {
            // if key does NOT exist in dictionary skip it
            if (!levelTileMapsDictionary.ContainsKey(mapData.key))
            {
                Debug.LogError("Found saved data for tilemap called '" + mapData.key + "', but Tilemap does not exist in scene.");
                continue;
            }

            // get according map
            var map = levelTileMapsDictionary[mapData.key];

            // clear map
            map.ClearAllTiles();

            if (mapData.tiles != null && mapData.tiles.Count > 0)
            {
                foreach (var tile in mapData.tiles)
                {

                    if (guidToTileBase.ContainsKey(tile.guidForBuildable))
                    {
                        map.SetTile(tile.position, guidToTileBase[tile.guidForBuildable]);
                    }
                    else
                    {
                        Debug.LogError("Refernce " + tile.guidForBuildable + " could not be found.");
                    }

                }
            }
        }
    }
}


[Serializable]
public class TilemapData
{
    public string key; // the key of your dictionary for the tilemap - here: the name of the map in the hierarchy
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo
{
    public string guidForBuildable;
    public Vector3Int position;

    public TileInfo(Vector3Int pos, string guid)
    {
        position = pos;
        guidForBuildable = guid;
    }
}