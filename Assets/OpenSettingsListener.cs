using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenSettingsListener : MonoBehaviour
{
    [SerializeField] LevelEditorInputReader LevelEditorInputReader;
    public void Start()
    {
        LevelEditorInputReader.OpenSettingsEvent += HandleOpenSettings;
    }

    private void HandleOpenSettings(bool obj)
    {
        // TODO: will components keep working after inactivated?
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        Debug.Log("Settings visibility toggled");
    }

    // TODO: unsubscribe correctly
}
