using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveHandler : Singleton<SaveHandler>
{
    public static Dictionary<TileBase, BuildingObjectBase> tileBaseToBuildingObject = new Dictionary<TileBase, BuildingObjectBase>();
    Dictionary<string, TileBase> guidToTileBase = new Dictionary<string, TileBase>();

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
            Debug.Log("saving level");
            level.PrepareToSave(tileBaseToBuildingObject);
            foreach (var item in level.data)
            {
                Debug.Log("key: " + item.key);
            }
        }
        FileHandler.SaveToJSON(campaign, NormalizeFileName(campaign.campaignName));
    }

    public void OnLoad()
    {
        Level level = LevelManager.GetInstance().GetActiveLevel();
        Level data = FileHandler.ReadFromJSON<Campaign>(NormalizeFileName(CampaignManager.GetInstance().ActiveCampaign.campaignName)).levels[0];
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

    public void LoadLevel(Level levelData)
    {
        Level level = LevelManager.GetInstance().GetActiveLevel();
        // For Faster lookups convert serializable List<Tuple>> to a dictionary 
        Dictionary<string, Tilemap> levelTileMapsDictionary = level.tilemaps.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

        foreach (var mapData in levelData.data)
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

    public static string NormalizeFileName(string campaignName)
    {

        // Convert to lowercase
        string normalized = campaignName.ToLower();
        // Replace spaces with hyphens
        normalized = normalized.Replace(' ', '-');
        Debug.Log("Normalized: " + normalized);
        return normalized + ".json";
    }

    public void InitializeLevels(List<Level> levels)
    {
        // Load Tilemaps
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach (Level level in levels)
        {
            foreach (var map in maps)
            {
                level.tilemaps.Add(Tuple.Create(map.name, map));
            }
        }

    }
}
