using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenuAttribute(fileName = "BuildingCategory", menuName = "Level Building/Create Category")]
public class BuildingCategory : ScriptableObject
{
    [SerializeField] public string TileMapName { get; private set; }
    [SerializeField] public PlaceType PlaceType { get; private set; }
    [SerializeField] int sortingOrder = 0;
    Tilemap tilemap;
}

public enum PlaceType
{
    None,
    Single,
    Line,
    Rectangle
}