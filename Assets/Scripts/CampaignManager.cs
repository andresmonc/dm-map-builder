using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : Singleton<CampaignManager>
{
    public Campaign ActiveCampaign { get; private set; }
    [SerializeField] public GameObject campaignLoadScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (ActiveCampaign == null && (!NetworkManager.Singleton.IsListening ||NetworkManager.Singleton.IsHost))
        {
            campaignLoadScreen.gameObject.SetActive(true);
        }
    }

    public List<Campaign> GetCampaigns()
    {
        List<Campaign> campaigns = new List<Campaign>
        {
            new Campaign("Lost Mines of Phandelvar"),
            new Campaign("The Twin Towers"),
            new Campaign("Return of the King")
        };
        return campaigns;
    }

    public void LoadCampaign(Campaign campaign)
    {
        ActiveCampaign = FileHandler.ReadFromJSON<Campaign>(SaveHandler.NormalizeFileName(campaign.campaignName));
        campaignLoadScreen.gameObject.SetActive(false);
        LevelManager.GetInstance().LoadLastModifiedLevel();
    }

    public void AddLevel(Level level)
    {
        ActiveCampaign.levels.Add(level);
    }
}

[Serializable]
public class Campaign
{
    public string campaignName;
    public string lastModified;

    public List<Level> levels;

    public Campaign(string name)
    {
        campaignName = name;
        lastModified = "10/24/1993/06:59am";
        levels = new List<Level>();
    }
}