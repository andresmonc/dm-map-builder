using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildableItem : MonoBehaviour
{
    [SerializeField] Image image;

    private BuildingObjectBase buildingObjectBase;

    private static TileMapEditor tileMapEditor;
    public void Initialize(Sprite sprite, BuildingObjectBase buildingObjectBase)
    {
        image.sprite = sprite;
        this.buildingObjectBase = buildingObjectBase;
    }

    public void SelectBuildable()
    {
        if (tileMapEditor == null)
        {
            tileMapEditor = TileMapEditor.GetInstance();
        }
        tileMapEditor.ObjectSelected(buildingObjectBase);
    }
}
