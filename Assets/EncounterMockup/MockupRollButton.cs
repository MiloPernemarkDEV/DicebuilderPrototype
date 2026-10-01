using EventChannels;
using UnityEngine;
using UnityEngine.UI;

namespace EncounterMockup
{
    public class MockupRollButton : MonoBehaviour
    {
        [SerializeField] private SO_EventEmptyPayload _rollCommandEvent;
        private Button _rollButton;

        private void Awake()
        {
            _rollButton = GetComponent<Button>();
        }
        private void OnEnable()
        {
            _rollButton.onClick.AddListener(HandlePressed);
        }
        private void OnDisable()
        {
            _rollButton.onClick.RemoveAllListeners();
        }

        private void HandlePressed()
        {
            _rollButton.interactable = false;
            _rollCommandEvent.TriggerEvent();
        }

    }
}