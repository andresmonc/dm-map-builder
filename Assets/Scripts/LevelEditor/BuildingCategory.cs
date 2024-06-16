using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenuAttribute(fileName = "BuildingCategory", menuName = "Level Building/Create Category")]
public class BuildingCategory : ScriptableObject
{
    [field: SerializeField] public string TileMapName { get; private set; }
    [field: SerializeField] public PlaceType PlaceType { get; private set; }
    [field: SerializeField] public int SortingOrder { get; private set; }
    public Tilemap Tilemap { get; set; }
}

public enum PlaceType
{
    None,
    Single,
    Line,
    Rectangle
}