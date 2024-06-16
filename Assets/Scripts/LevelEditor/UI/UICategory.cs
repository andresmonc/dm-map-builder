using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UICategory", menuName = "Level Building/Create UI Category")]
public class UICategory : ScriptableObject
{
    [field: SerializeField] public int siblingIndex { get; private set; }
    [field: SerializeField] public Color BackgroundColor { get; private set; }

}
