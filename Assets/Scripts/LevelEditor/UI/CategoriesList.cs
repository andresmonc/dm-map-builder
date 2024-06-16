using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
