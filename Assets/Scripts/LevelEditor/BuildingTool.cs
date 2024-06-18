using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildable", menuName = "Level Building/Create Tool")]
public class BuildingTool : BuildingObjectBase
{
    [SerializeField] private ToolType toolType;

    public void Use()
    {
        ToolController tc = ToolController.GetInstance();
        switch (toolType)
        {
            case ToolType.Eraser:
                tc.Eraser();
                break;
        }
    }
}

enum ToolType
{
    None,
    Eraser
}