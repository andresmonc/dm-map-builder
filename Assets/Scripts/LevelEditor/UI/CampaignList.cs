using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignList : MonoBehaviour
{
    [SerializeField] GameObject campaignItemPrefab;
    [SerializeField] Transform wrapperElement;
    void Start()
    {
        ;
        foreach (Campaign campaign in CampaignManager.GetInstance().GetCampaigns())
        {
            GameObject gameObject = Instantiate(campaignItemPrefab, wrapperElement);
            gameObject.GetComponent<CampaignItem>().Initialize(campaign);
        }
    }
}
