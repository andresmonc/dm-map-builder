using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour
{
    [SerializeField] BuildingObjectBase item;
    private Button button;

    private BuildingCreator buildingCreator;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        buildingCreator = BuildingCreator.GetInstance();
    }


    private void ButtonClicked()
    {
        Debug.Log("Button was clicked: " + item.name);
        buildingCreator.ObjectSelected(item);
    }
}
