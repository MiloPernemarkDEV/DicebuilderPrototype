using DiceTools;
using UnityEngine;

namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventRuntimeDiePayload", menuName = "Event Channels/Runtime Die Payload")]
    public class SO_EventRuntimeDiePayload : ScriptableObject
    {
        public event System.Action<RuntimeDie> OnEventTriggered;
        public void TriggerEvent(RuntimeDie payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
}


