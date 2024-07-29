using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Tank Player Input Reader", menuName = "Input/Tank Player Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{

    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;
    public Vector2 AimPositon { get; private set; }
    private Controls controls;
    public event Action<float> ScrollEvent;
    public event Action<bool> OpenSettingsEvent;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPositon = context.ReadValue<Vector2>();
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

    public void OnOpenSettings(InputAction.CallbackContext context)
    {
        OpenSettingsEvent?.Invoke(true);
    }
}
