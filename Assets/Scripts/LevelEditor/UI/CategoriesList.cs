using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CategoriesList : MonoBehaviour
{
    [SerializeField] List<UICategory> categories;
    [SerializeField] Transform wrapperElement;
    [SerializeField] GameObject categoryPrefab;
    [SerializeField] GameObject itemPrefab;

    Dictionary<UICategory, GameObject> uiElements = new Dictionary<UICategory, GameObject>();
    private void Start()
    {
        foreach (UICategory category in categories)
        {
            if (!uiElements.ContainsKey(category))
            {
                GameObject inst = Instantiate(categoryPrefab, wrapperElement);
                uiElements[category] = inst;
            }
            GameObject categoryGameObject = uiElements[category];
            categoryGameObject.GetComponent<CategoryItem>().Initialize(
                category.siblingIndex,
                category.BackgroundColor,
                category.name
            );
        }

        BuildingObjectBase[] buildables = GetAllBuildables();
        foreach (BuildingObjectBase buildable in buildables)
        {
            if (buildable == null) { continue; }
            Debug.Log(buildable.name);
            Debug.Log(buildable.UICategory);
            GameObject categoryParent = uiElements[buildable.UICategory];
            GameObject buildableItem = Instantiate(itemPrefab, categoryParent.transform);
            Tile tile = (Tile)buildable.TileBase;
            buildableItem.GetComponent<BuildableItem>().Initialize(tile.sprite);
        }
    }

    private BuildingObjectBase[] GetAllBuildables()
    {
        return Resources.LoadAll<BuildingObjectBase>("Scriptables/Buildables");
    }
}
