using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildable", menuName = "Level Building/Create Tool")]
public class BuildingTool : BuildingObjectBase
{
    [SerializeField] private ToolType toolType;

    public void Use(Vector3Int position)
    {
        ToolController tc = ToolController.GetInstance();
        switch (toolType)
        {
            case ToolType.Eraser:
                tc.Eraser(position);
                break;
            default:
                Debug.LogError("Tool type not defined!");
                break;
        }
    }
}

enum ToolType
{
    None,
    Eraser
}