using UnityEngine;
using UnityEngine.InputSystem;

public static class InputUtils
{
    private static float pressStartTime;
    private static bool hasTriggered;
    
    public static bool TryGetPointerPosition(out Vector2 position)
    {
        if (Touchscreen.current?.primaryTouch.press.isPressed == true)
        {
            position = Touchscreen.current.primaryTouch.position.ReadValue();
            return true;
        }

        if (Mouse.current != null)
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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticVariables()
    {
        pressStartTime = -1f;
        hasTriggered = false;
    }
}