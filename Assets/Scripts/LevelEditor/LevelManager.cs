using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            CampaignManager.GetInstance().AddLevel(activeLevel);
            return;
        }

        // load previous memory level but first find the one with the most recent last modified
        Level lastModifiedLevel = null;
        DateTime latestSaveTime = DateTime.MinValue;
        string format = "M/d/yyyy h:mm tt";
        InitializeLevels(campaign.levels);
        foreach (Level level in campaign.levels)
        {
            if (level.saveTime == null)
            {
                continue;
            }
            DateTime levelSaveTime = DateTime.ParseExact(level.saveTime, format, CultureInfo.InvariantCulture);
            if (levelSaveTime > latestSaveTime)
            {
                latestSaveTime = levelSaveTime;
                lastModifiedLevel = level;
            }
        }
        activeLevel = lastModifiedLevel;
        SaveHandler.GetInstance().LoadLevel(lastModifiedLevel);
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
