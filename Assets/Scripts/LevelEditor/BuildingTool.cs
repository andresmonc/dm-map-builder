using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Buildable", menuName = "Level Building/Create Tool")]
public class BuildingTool : BuildingObjectBase
{
    [SerializeField] private ToolType toolType;
    [field: SerializeField] public Tile ToolTile { get; private set; }


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