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
        List<Level> levels = campaign.levels;
        if (levels == null || levels.Count == 0)
        {
            InitializeLevel();
        }
        else
        {
            Debug.Log("ELSE HIT!");
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
            level.tilemaps.Add(Tuple.Create(map.name, map));
        }
        activeLevel = level;
        CampaignManager.GetInstance().AddLevel(activeLevel);
    }
}
