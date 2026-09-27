using UnityEngine;
using HubBuilding;
using EventChannels;

public class ExampleItemPlacementListener : MonoBehaviour
{
    [SerializeField] private SO_EventSO_HubItemPayload _itemPlacementEvent;

    private void OnEnable()
    {
        _itemPlacementEvent.OnEventTriggered += HandleItemPlacement;
    }
    private void OnDisable()
    {
        _itemPlacementEvent.OnEventTriggered -= HandleItemPlacement;
    }

    private void HandleItemPlacement(SO_HubItem itemSO)
    {
        Debug.Log($"### {name}: Item placed, ID: {itemSO.ID}");
    }
}
