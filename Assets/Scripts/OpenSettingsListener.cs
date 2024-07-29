using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenSettingsListener : MonoBehaviour
{
    [SerializeField] LevelEditorInputReader levelEditorInputReader;
    [SerializeField] InputReader playerInputReader;

    public void Start()
    {
        if (levelEditorInputReader != null)
        {
            levelEditorInputReader.OpenSettingsEvent += HandleOpenSettings;
        }
        else if (playerInputReader != null)
        {
            playerInputReader.OpenSettingsEvent += HandleOpenSettings;
        }
    }

    private void HandleOpenSettings(bool obj)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // TODO: unsubscribe correctly
}
