using UnityEngine;
using EventChannels;
using HubBuilding;
using UnityEngine.UI;

public class ExampleItemPlacementTriggerer : MonoBehaviour
{
    [SerializeField] private SO_HubItem _itemSO;
    [SerializeField] private SO_EventSO_HubItemPayload _itemPlacementEvent;
    [SerializeField] private Button _itemPlacementButton;

    private void OnEnable()
    {
        _itemPlacementButton.onClick.AddListener(HandleButtonPressed);
    }
    private void OnDisable()
    {
        _itemPlacementButton.onClick.RemoveAllListeners();
    }

    private void HandleButtonPressed()
    {
        _itemPlacementButton.interactable = false;
        _itemPlacementEvent.TriggerEvent(_itemSO);
    }

}
