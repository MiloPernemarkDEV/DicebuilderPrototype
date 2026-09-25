using UnityEngine;

namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventEmptyPayload", menuName = "Event Channels/Empty Payload")]
    public class SO_EventEmptyPayload : ScriptableObject
    {
        public event System.Action OnEventTriggered;
        public void TriggerEvent()
        {
            OnEventTriggered?.Invoke();
        }
    }
}