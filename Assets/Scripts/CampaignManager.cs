using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : Singleton<CampaignManager>
{
    public Campaign ActiveCampaign { get; private set; }
    [SerializeField] public GameObject campaignLoadScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (ActiveCampaign == null)
        {
            campaignLoadScreen.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("Loading campaign: " + campaign.campaignName);
        ActiveCampaign = campaign;
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