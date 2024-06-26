using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CampaignItem : MonoBehaviour
{
    [SerializeField] private TMP_Text campaignNameText;
    [SerializeField] private TMP_Text lastModifiedText;
    private Campaign campaign;

    public void Initialize(Campaign campaign)
    {
        campaignNameText.text = campaign.campaignName;
        lastModifiedText.text = campaign.lastModified;
        this.campaign = campaign;
    }

    public void LoadCampaign()
    {
        CampaignManager.GetInstance().LoadCampaign(campaign);
    }

}
