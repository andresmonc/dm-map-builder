using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileMapEditor : Singleton<TileMapEditor>
{
    [SerializeField] Tilemap previewMap, defaultMap;

    [SerializeField] private LevelEditorInputReader inputReader;

    TileBase tileBase;
    Vector2 mousePos;

    Vector3Int currentGridPos;
    Vector3Int lastGridPosition;
    Camera _camera;

    bool isPosOverGameObject = false;

    private BuildingObjectBase selectedObject;

    public BuildingObjectBase SelectedObject
    {
        get { return selectedObject; }
        set
        {
            selectedObject = value;
            tileBase = selectedObject != null ? selectedObject.TileBase : null;
            UpdatePreview();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
        Debug.Log("BuildingCreator awoken");
    }

    private void Update()
    {
        // if nothing is selected - show preview
        if (SelectedObject == null) { return; }
        // Get mouse position from input reader
        mousePos = inputReader.MousePosition;
        Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
        Vector3Int gridPos = previewMap.WorldToCell(pos);
        if (gridPos != currentGridPos)
        {
            lastGridPosition = currentGridPos;
            currentGridPos = gridPos;
            UpdatePreview();
        }
        isPosOverGameObject = EventSystem.current.IsPointerOverGameObject();
    }

    private void OnEnable()
    {
        inputReader.LeftClickEvent += HandleMouseLeftClick;
        inputReader.RightClickEvent += HandleMouseRightClick;

    }

    private void OnDisable()
    {
        inputReader.LeftClickEvent -= HandleMouseLeftClick;
        inputReader.RightClickEvent -= HandleMouseRightClick;
    }

    private void HandleMouseRightClick(bool click)
    {
        if (selectedObject == null || !click) { return; }
        selectedObject = null;
        previewMap.SetTile(lastGridPosition, null);
        previewMap.SetTile(currentGridPos, null);
    }

    private void HandleMouseLeftClick(bool click)
    {
        if (selectedObject != null && !isPosOverGameObject && click)
        {
            HandleDrawing();
        }
    }

    public void ObjectSelected(BuildingObjectBase obj)
    {
        SelectedObject = obj;

        // set preview where mouse is
        // on click draw tile on tile map
        // on right click cancel drawing
    }

    private void UpdatePreview()
    {
        // remove old tile
        previewMap.SetTile(lastGridPosition, null);
        // set current tile to current pos
        previewMap.SetTile(currentGridPos, tileBase);
    }

    private void HandleDrawing()
    {
        DrawItem();
    }

    private void DrawItem()
    {
        defaultMap.SetTile(currentGridPos, tileBase);
    }

}
