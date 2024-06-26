using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : Singleton<CampaignManager>
{
    private static Campaign activeCampaign;
    [SerializeField] public GameObject campaignLoadScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (activeCampaign == null)
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
        Debug.Log("Loading campaign: " + campaign.CampaignName);
    }

    internal static void AddLevel(Level level)
    {
        activeCampaign.Levels.Add(level);
    }
}

public class Campaign
{
    public string CampaignName { get; private set; }
    public string LastModified { get; private set; }

    public List<Level> Levels { get; private set; }

    public Campaign(string name)
    {
        CampaignName = name;
        LastModified = "10/24/1993/06:59am";
    }
}