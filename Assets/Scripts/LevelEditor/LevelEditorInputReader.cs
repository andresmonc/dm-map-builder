using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;


[CreateAssetMenu(fileName = "New Level Editor Input Reader", menuName = "Input/Input Reader")]
public class LevelEditorInputReader : ScriptableObject, ILevelEditorPlayerActions
{
    Controls controls;
    public event Action<bool> LeftClickEvent;
    public event Action<bool> RightClickEvent;
    public Vector2 MousePosition { get; private set; }


    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
        }
        controls.LevelEditorPlayer.Enable();
        Debug.Log("Subscribing to changes!");
        controls.LevelEditorPlayer.MousePosition.performed += OnMousePosition;
        controls.LevelEditorPlayer.MouseLeftClick.performed += OnMouseLeftClick;
        controls.LevelEditorPlayer.MouseRightClick.performed += OnMouseRightClick;

    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing to changes!");
        controls.LevelEditorPlayer.MousePosition.performed -= OnMousePosition;
        controls.LevelEditorPlayer.MouseLeftClick.performed -= OnMouseLeftClick;
        controls.LevelEditorPlayer.MouseRightClick.performed -= OnMouseRightClick;
    }


    public void OnMouseRightClick(InputAction.CallbackContext context)
    {
        SendClickEvent(context, RightClickEvent);
    }

    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        SendClickEvent(context, LeftClickEvent);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }


    private void SendClickEvent(InputAction.CallbackContext context, Action<bool> clickAction)
    {
        if (context.performed)
        {
            clickAction?.Invoke(true);
        }
        else if (context.canceled)
        {
            clickAction?.Invoke(false);
        }
    }

}
