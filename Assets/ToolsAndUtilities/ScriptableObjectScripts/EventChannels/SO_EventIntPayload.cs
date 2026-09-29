using UnityEngine;

namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventIntPayload", menuName = "Event Channels/Integer Payload")]
    public class SO_EventIntPayload : ScriptableObject
    {
        public event System.Action<int> OnEventTriggered;
        public void TriggerEvent(int payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
}


