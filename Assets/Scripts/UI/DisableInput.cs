using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInput : MonoBehaviour
{
    [SerializeField] LevelEditorInputReader levelEditorInputReader;
    private void OnEnable()
    {
        levelEditorInputReader.DisableInput();
    }

    private void OnDisable()
    {
        levelEditorInputReader.EnableInput();
    }
}
