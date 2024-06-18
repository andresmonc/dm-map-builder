using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class TilemapGridLines : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap; // Reference to the Tilemap
    [SerializeField] public Material lineMaterial; // Material for the lines
    [SerializeField] public float lineWidth = 0.05f; // Width of the lines
    [SerializeField] Vector3Int minBounds;
    [SerializeField] Vector3Int maxBounds;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 0;

        DrawGridLines();
    }

    void DrawGridLines()
    {
        // Prepare positions list
        var positions = new List<Vector3>();

        // Draw vertical lines
        for (int x = minBounds.x; x <= maxBounds.x; x++)
        {
            positions.Add(tilemap.CellToWorld(new Vector3Int(x, minBounds.y, 0)));
            positions.Add(tilemap.CellToWorld(new Vector3Int(x, maxBounds.y + 1, 0)));
        }

        // Draw horizontal lines
        for (int y = minBounds.y; y <= maxBounds.y; y++)
        {
            positions.Add(tilemap.CellToWorld(new Vector3Int(minBounds.x, y, 0)));
            positions.Add(tilemap.CellToWorld(new Vector3Int(maxBounds.x + 1, y, 0)));
        }

        // Apply positions to line renderer
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}