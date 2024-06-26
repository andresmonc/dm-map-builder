using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : Singleton<LevelManager>
{
    private Level activeLevel;

    public void LoadLastModifiedLevel()
    {
        Campaign campaign = CampaignManager.GetInstance().ActiveCampaign;
        List<Level> levels = campaign.Levels;
        if (levels == null || levels.Count == 0)
        {
            InitializeLevel();
        }
        else
        {
            // load previous memory level but first find the one with the most recent last modified
        }
        Debug.Log(activeLevel);
    }

    public Level GetActiveLevel()
    {
        return activeLevel;
    }


    public void InitializeLevel()
    {
        // Load Tilemaps
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        Level level = new Level();
        foreach (var map in maps)
        {
            level.tilemaps.Add(map.name, map);
        }
        activeLevel = level;
        CampaignManager.GetInstance().AddLevel(activeLevel);
    }
}

public class Level
{
    public Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    public DateTime LastModified { get; private set; }

}
