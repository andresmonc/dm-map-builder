using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text text;



    public void Initialize(int siblingIndex, Color color, string name)
    {
        this.name = name;
        text.text = name;
        transform.SetSiblingIndex(siblingIndex);
        image.color = color;
    }
}
