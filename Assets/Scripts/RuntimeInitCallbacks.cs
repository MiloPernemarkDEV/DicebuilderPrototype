using System;
using System.Reflection;
using UnityEngine;
using Utilities;

public static class RuntimeInitCallbacks
{
    /// <summary>
    /// Disables logs in mobile builds
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void DisableLogs()
    {
#if UNITY_IOS || UNITY_ANDROID
            Debug.unityLogger.logEnabled = false;
#else
        Debug.unityLogger.logEnabled = true;
#endif  
    } 
    
#if UNITY_EDITOR
    /// <summary>
    /// Default Initializes static fields in editor only
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitializeStaticFields()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo prop in type.GetProperties(flags))
            {
                var attr = prop.GetCustomAttribute<DefaultInitStaticField>();
                if (attr == null) continue; 
                ResetProperties(prop);
            }

            foreach (FieldInfo field in type.GetFields(flags))
            {
                var attr = field.GetCustomAttribute<DefaultInitStaticField>();
                if (attr == null) continue;
                ResetFields(field);
            }
        }
    }
    
    private static void ResetFields(FieldInfo field)
    {
        Type t = field.FieldType;

        if (t == typeof(float)) field.SetValue(null, 0.0f);
        else if (t == typeof(int)) field.SetValue(null, 0);
        else if (t == typeof(string)) field.SetValue(null, string.Empty);
        else field.SetValue(null, null); 
    }

    private static void ResetProperties(PropertyInfo prop)
    {
        Type t = prop.PropertyType;
        if (t == typeof(float)) prop.SetValue(null, 0.0f);
        else if (t == typeof(int)) prop.SetValue(null, 0);
        else if (t == typeof(string)) prop.SetValue(null, string.Empty);
        else prop.SetValue(null, null);
    }
    
#endif
}
