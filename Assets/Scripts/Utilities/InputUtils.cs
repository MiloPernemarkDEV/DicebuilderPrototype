using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public static class InputUtils
{
    [DefaultInitStaticField] private static float pressStartTime;
    [DefaultInitStaticField] private static bool hasTriggered;
    
    public static bool TryGetPointerPosition(out Vector2 outPos)
    {
        if (Touchscreen.current?.primaryTouch.press.isPressed == true)
        {
            outPos = Touchscreen.current.primaryTouch.position.ReadValue();
            return true;
        }

        if (Mouse.current != null)
        {
            outPos = Mouse.current.position.ReadValue();
            return true;
        }

        outPos = default;
        return false;
    }

    public static bool WasPressedThisFrame()
    {
        return Mouse.current?.leftButton.wasPressedThisFrame == true ||
               Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true;
    }

    public static bool HasBeenPressedFor(float duration)
    {
        var isPressed = Mouse.current?.leftButton.isPressed == true ||
                         Touchscreen.current?.primaryTouch.press.isPressed == true;
        
        if (isPressed)
        {
            if (pressStartTime < 0.0f)
            {
                pressStartTime = Time.time;
            }

            if (!hasTriggered && (Time.time - pressStartTime >= duration))
            {
                hasTriggered = true;
                return true;
            }
        }
        else
        {
            pressStartTime = -1.0f;
            hasTriggered = false;   
        }
        return false; 
    }
}