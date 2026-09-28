using HubBuilding;
using UnityEngine;
namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventSO_HubItemPayload", menuName = "Event Channels/SO_HubItem Payload")]
    public class SO_EventHubItemMovedPayload : ScriptableObject
    {
        public event System.Action<HubItemMovedData> OnEventTriggered;
        public void TriggerEvent(HubItemMovedData payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
}