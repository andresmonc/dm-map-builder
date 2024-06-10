using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    private IEnumerable<ISaveable> saveables;

    [Header("References")]
    [SerializeField] private float saveFrequency = 60f * 5f;


    public static SaveManager Instance
    {
        get
        {
            if (instance != null) { return instance; }
            instance = FindObjectOfType<SaveManager>();
            if (instance == null)
            {
                Debug.LogError("No SaveManager in scene!");
            }
            return instance;
        }
    }

    private void registerSaveables()
    {
        saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

    }

}
