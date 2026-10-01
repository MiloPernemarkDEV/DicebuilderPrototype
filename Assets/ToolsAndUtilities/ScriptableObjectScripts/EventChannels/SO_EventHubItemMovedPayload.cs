using HubBuilding;
using UnityEngine;
namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventHubItemMovedPayload", menuName = "Event Channels/SO_EventHubItemMovedPayload")]
    public class SO_EventHubItemMovedPayload : ScriptableObject
    {
        public event System.Action<HubItemMovedData> OnEventTriggered;
        public void TriggerEvent(HubItemMovedData payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
}