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
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    // TODO: unsubscribe correctly
}
