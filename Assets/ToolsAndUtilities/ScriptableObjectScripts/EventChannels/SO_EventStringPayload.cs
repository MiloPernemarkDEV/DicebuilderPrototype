using UnityEngine;

namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventStringPayload", menuName = "Event Channels/String Payload")]
    public class SO_EventStringPayload : ScriptableObject
    {
        public event System.Action<string> OnEventTriggered;
        public void TriggerEvent(string payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
}