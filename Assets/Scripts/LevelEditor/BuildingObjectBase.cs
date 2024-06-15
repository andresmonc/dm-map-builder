using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Buildable", menuName = "Building Objects/Create Buildable")]
public class BuildingObjectBase : ScriptableObject
{
    [field: SerializeField] public Category Category { get; private set; }
    [field: SerializeField] public TileBase TileBase { get; private set; }

}

public enum Category
{
    Wall,
    Floor
}