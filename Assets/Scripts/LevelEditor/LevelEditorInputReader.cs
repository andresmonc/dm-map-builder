using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;


[CreateAssetMenu(fileName = "New Level Editor Input Reader", menuName = "Input/Level Editor Input Reader")]
public class LevelEditorInputReader : ScriptableObject, ILevelEditorPlayerActions
{
    Controls controls;
    public event Action<InputAction.CallbackContext> LeftClickEvent;
    public event Action<bool> RightClickEvent;

    public event Action<float> ScrollEvent;
    public Vector2 MousePosition { get; private set; }


    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.LevelEditorPlayer.SetCallbacks(this);
        }
        controls.LevelEditorPlayer.Enable();
    }


    public void OnMouseRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RightClickEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            RightClickEvent?.Invoke(false);
        }
    }

    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        LeftClickEvent?.Invoke(context);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnMouseScroll(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<Vector2>().y;
        if (value > 0)
        {
            ScrollEvent?.Invoke(1);
        }
        else if (value < 0)
        {
            ScrollEvent?.Invoke(-1);
        }
    }
}
