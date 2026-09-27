using HubBuilding;
using UnityEngine;
namespace EventChannels
{
    [CreateAssetMenu(fileName = "SO_EventSO_HubItemPayload", menuName = "Event Channels/SO_HubItem Payload")]
    public class SO_EventSO_HubItemPayload : ScriptableObject
    {
        public event System.Action<SO_HubItem> OnEventTriggered;
        public void TriggerEvent(SO_HubItem payload)
        {
            OnEventTriggered?.Invoke(payload);
        }
    }
}


