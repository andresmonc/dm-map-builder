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
        sceneMaps = FindObjectsOfType<Tilemap>().ToList()
            .FindAll(map => { return map != excludedMaps.Contains(map); })
            .OrderBy(map => map.GetComponent<Renderer>().sortingOrder).ToList();
    }
    public void Eraser(Vector3Int position)
    {
        for (int i = sceneMaps.Count - 1; i >= 0; i--)
        {
            Tilemap map = sceneMaps[i];
            if (map.HasTile(position))
            {
                map.SetTile(position, null);
                break;
            };
        }
    }
}
