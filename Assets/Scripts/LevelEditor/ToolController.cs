using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class ToolController : Singleton<ToolController>
{

    [SerializeField] List<Tilemap> toolMapExclusions;

    List<Tilemap> sceneMaps;

    private void Start()
    {
        HashSet<Tilemap> excludedMaps = toolMapExclusions.ToHashSet();
        sceneMaps = FindObjectsOfType<Tilemap>().ToList().FindAll(map => { return map != excludedMaps.Contains(map); });
        sceneMaps.ForEach(map =>
        {
            Debug.Log(map.name);
        });
    }
    public void Eraser()
    {
        Debug.Log("Erasing");
    }
}
