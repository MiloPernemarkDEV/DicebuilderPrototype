using UnityEngine;
using UnityEngine.InputSystem;

public static class InputUtils
{
    public static bool TryGetPointerPosition(out Vector2 position)
    {
        if (Touchscreen.current?.primaryTouch.press.isPressed == true)
        {
            position = Touchscreen.current.primaryTouch.position.ReadValue();
            return true;
        }

        if (Mouse.current?.leftButton.isPressed == true)
        {
            position = Mouse.current.position.ReadValue();
            return true;
        }

        position = default;
        return false;
    }

    public static bool WasPressedThisFrame()
    {
        return Mouse.current?.leftButton.wasPressedThisFrame == true ||
               Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true;
    }
}