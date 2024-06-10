using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    private IEnumerable<ISaveable> saveables;
    private static string path = Application.persistentDataPath + "/gamesave";
    private static BinaryFormatter formatter = new BinaryFormatter();


    [Header("References")]
    [SerializeField] private float saveFrequency = 60f * 5f; // Default to 5 minutes

    private void Start()
    {
        // Find and register saveables. Very expensive but should only happen on start.
        RegisterSaveables();

        // Start the periodic saving
        InvokeRepeating(nameof(ScheduleSerializeSaveables), saveFrequency, saveFrequency);
    }

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

    private void RegisterSaveables()
    {
        saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
    }

    private async void ScheduleSerializeSaveables()
    {
        await SerializeSaveables();
    }

    private async Task SerializeSaveables()
    {
        List<SaveData> saveData = new List<SaveData>();
        foreach (ISaveable saveable in saveables)
        {
            saveData.Add(saveable.Save());
        }
        SaveDataContainer container = new SaveDataContainer { SaveData = saveData };

        await Task.Run(() =>
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fileStream, container);
            }
        });
    }

    private void DeserializeSaveables()
    {
        if (File.Exists(path))
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                SaveDataContainer container = (SaveDataContainer)formatter.Deserialize(fileStream);
                foreach (SaveData saveData in container.SaveData)
                {
                    // Saveable.GetByID(xxxx).Load(saveData)?                    
                }
            }
        }
    }
}