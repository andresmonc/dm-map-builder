using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingCreator : Singleton<BuildingCreator>
{
    [SerializeField] Tilemap previewMap, defaultMap;
    TileBase tileBase;
    Controls controls;
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

    // MOVE TO INPUTREADER :l
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
        }
        controls.LevelEditorPlayer.Enable();
        Debug.Log("Subscribing to changes!");
        controls.LevelEditorPlayer.MousePosition.performed += HandleMouseMove;
        controls.LevelEditorPlayer.MouseLeftClick.performed += HandleMouseLeftClick;
        controls.LevelEditorPlayer.MouseRightClick.performed += HandleMouseRightClick;

    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing to changes!");
        controls.LevelEditorPlayer.MousePosition.performed -= HandleMouseMove;
        controls.LevelEditorPlayer.MouseLeftClick.performed -= HandleMouseLeftClick;
        controls.LevelEditorPlayer.MouseRightClick.performed -= HandleMouseRightClick;
    }


    private void HandleMouseRightClick(InputAction.CallbackContext context)
    {
        if (selectedObject == null) { return; }
        selectedObject = null;
    }

    private void HandleMouseLeftClick(InputAction.CallbackContext context)
    {
        if (selectedObject != null && !isPosOverGameObject)
        {
            HandleDrawing();
        }
    }

    private void HandleMouseMove(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    // MOVE TO INPUTREADER :l

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
        Debug.Log("handling drawing!");
        DrawItem();
    }

    private void DrawItem()
    {
        defaultMap.SetTile(currentGridPos, tileBase);
    }



}
