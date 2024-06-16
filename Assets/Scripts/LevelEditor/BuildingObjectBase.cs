using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Buildable", menuName = "Level Building/Create Buildable")]
public class BuildingObjectBase : ScriptableObject
{
    [field: SerializeField] public BuildingCategory Category { get; private set; }
    [field: SerializeField] public TileBase TileBase { get; private set; }
    [field: SerializeField] public UICategory UICategory { get; private set; }
    [SerializeField] private PlaceType placeType;
    public PlaceType PlaceType
    {
        get
        {
            return placeType == PlaceType.None ? Category.PlaceType : placeType;
        }
    }


}
