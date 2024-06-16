using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildableItem : MonoBehaviour
{
    [SerializeField] Image image;

    public void Initialize(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
