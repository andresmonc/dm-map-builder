using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
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

    bool holdActive = false;
    bool isPosOverGameObject = false;

    private BuildingObjectBase selectedObject;
    private Vector3Int holdStartPosition;
    BoundsInt bounds;

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
    }

    private void Update()
    {
        isPosOverGameObject = EventSystem.current.IsPointerOverGameObject();
        // if something is selected - show preview
        if (selectedObject != null)
        {
            mousePos = inputReader.MousePosition;
            Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if (gridPos != currentGridPos)
            {
                lastGridPosition = currentGridPos;
                currentGridPos = gridPos;
                UpdatePreview();
                if (holdActive)
                {
                    HandleDrawing();
                }
            }
        }
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
        DrawItem(previewMap, lastGridPosition, null);
        DrawItem(previewMap, currentGridPos, null);

    }

    private void HandleMouseLeftClick(InputAction.CallbackContext ctx)
    {
        if (selectedObject != null && !isPosOverGameObject)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                holdActive = true;
                if (ctx.interaction is TapInteraction)
                {
                    holdStartPosition = currentGridPos;
                }
                HandleDrawing();
            }
            else
            {
                if (ctx.interaction is SlowTapInteraction || ctx.interaction is TapInteraction && ctx.phase == InputActionPhase.Performed)
                {
                    holdActive = false;
                    HandleDrawRelease();
                }
            }
        }
    }

    public void ObjectSelected(BuildingObjectBase obj)
    {
        SelectedObject = obj;
    }

    private void UpdatePreview()
    {
        // remove old tile
        DrawItem(previewMap, lastGridPosition, null);

        // set current tile to current pos
        DrawItem(previewMap, currentGridPos, tileBase);

    }

    private void HandleDrawing()
    {
        switch (selectedObject.PlaceType)
        {
            case PlaceType.Single:
                DrawItem(DetermineTileMap(), currentGridPos, tileBase);
                break;
            case PlaceType.Line:
                LineRenderer();
                break;
            case PlaceType.Rectangle:
                RectangleRenderer();
                break;
        }
    }

    private void HandleDrawRelease()
    {
        if (selectedObject != null)
        {
            switch (selectedObject.PlaceType)
            {
                case PlaceType.Line:
                case PlaceType.Rectangle:
                    DrawBounds(DetermineTileMap());
                    previewMap.ClearAllTiles();
                    break;
            }
        }
    }

    private void RectangleRenderer()
    {
        previewMap.ClearAllTiles();
        bounds.xMin = currentGridPos.x < holdStartPosition.x ? currentGridPos.x : holdStartPosition.x;
        bounds.xMax = currentGridPos.x > holdStartPosition.x ? currentGridPos.x : holdStartPosition.x;
        bounds.yMin = currentGridPos.y < holdStartPosition.y ? currentGridPos.y : holdStartPosition.y;
        bounds.yMax = currentGridPos.y > holdStartPosition.y ? currentGridPos.y : holdStartPosition.y;
        DrawBounds(previewMap);
    }

    private void LineRenderer()
    {
        Debug.Log("Line renderer working");
        //  Render Preview on UI Map, draw real one on Release
        previewMap.ClearAllTiles();

        float diffX = Mathf.Abs(currentGridPos.x - holdStartPosition.x);
        float diffY = Mathf.Abs(currentGridPos.y - holdStartPosition.y);

        bool lineIsHorizontal = diffX >= diffY;

        if (lineIsHorizontal)
        {
            bounds.xMin = currentGridPos.x < holdStartPosition.x ? currentGridPos.x : holdStartPosition.x;
            bounds.xMax = currentGridPos.x > holdStartPosition.x ? currentGridPos.x : holdStartPosition.x;
            bounds.yMin = holdStartPosition.y;
            bounds.yMax = holdStartPosition.y;
        }
        else
        {
            bounds.xMin = holdStartPosition.x;
            bounds.xMax = holdStartPosition.x;
            bounds.yMin = currentGridPos.y < holdStartPosition.y ? currentGridPos.y : holdStartPosition.y;
            bounds.yMax = currentGridPos.y > holdStartPosition.y ? currentGridPos.y : holdStartPosition.y;
        }
        DrawBounds(previewMap);
    }

    private void DrawBounds(Tilemap map)
    {
        // Draws bounds on given map
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                DrawItem(map, new Vector3Int(x, y, 0), tileBase);
            }
        }
    }

    private Tilemap DetermineTileMap()
    {
        Debug.Log(selectedObject.Category.name);
        Debug.Log(selectedObject.Category.Tilemap.name);
        if (selectedObject.Category == null || selectedObject.Category.Tilemap == null)
        {
            return defaultMap;
        }
        else
        {
            return selectedObject.Category.Tilemap;
        }
    }

    private void DrawItem(Tilemap map, Vector3Int position, TileBase tileBase)
    {
        map.SetTile(position, tileBase);
    }

}
