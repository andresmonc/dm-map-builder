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
        Debug.Log(activeLevel);
    }

    public Level GetActiveLevel()
    {
        return activeLevel;
    }

    private void AddLevelToCampaign()
    {
        CampaignManager.GetInstance().AddLevel(GetActiveLevel());
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
    }
}

public class Level
{
    public Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    public DateTime LastModified { get; private set; }

}
