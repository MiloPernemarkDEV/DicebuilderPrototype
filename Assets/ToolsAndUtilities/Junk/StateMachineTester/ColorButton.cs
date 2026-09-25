using UnityEngine;
using UnityEngine.UI;
using EventChannels;

public class ColorButton : MonoBehaviour
{
    [SerializeField] private string _colorString = string.Empty;
    [SerializeField] private SO_EventStringPayload _pressedEvent;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonPressed);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void HandleButtonPressed()
    {
        _pressedEvent.TriggerEvent(_colorString);
    }

}
