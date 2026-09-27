using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Disables logs on mobile builds
    /// </summary>
    public static class RuntimeInitOnLoad
    {
        // A Callback for when the runtime is starting up and the first scene has been loaded
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void DisableLogs()
        {
#if UNITY_IOS || UNITY_ANDROID
            Debug.unityLogger.logEnabled = false;
#else
            Debug.unityLogger.logEnabled = true;
#endif  

        }
    }
}