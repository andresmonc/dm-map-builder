using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingCreator : Singleton<BuildingCreator>
{
    Controls controls;
    Vector2 mousePos;

    protected override void Awake()
    {
        base.Awake();
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.LevelEditorPlayer.MousePosition.performed += HandleMouseMove;
        controls.LevelEditorPlayer.MouseLeftClick.performed += HandleMouseLeftClick;
        controls.LevelEditorPlayer.MouseRightClick.performed += HandleMouseRightClick;

    }

    private void OnDisable()
    {
        controls.LevelEditorPlayer.MousePosition.performed -= HandleMouseMove;
        controls.LevelEditorPlayer.MouseLeftClick.performed -= HandleMouseLeftClick;
        controls.LevelEditorPlayer.MouseRightClick.performed -= HandleMouseRightClick;
    }


    private void HandleMouseRightClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void HandleMouseLeftClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void HandleMouseMove(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }



}
