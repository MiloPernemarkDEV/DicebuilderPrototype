using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using EventChannels;

namespace Encounter
{
    public class RollButton : MonoBehaviour, IPointerClickHandler, IRollCommandInterface
    {
        [SerializeField] private SO_EventEmptyPayload _rollCommandEvent;

        private TMP_Text _buttonText = null;

        private void Awake()
        {
            _buttonText = GetComponentInChildren<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData pData)
        {
            //...
        }

        public void SetRollCommandEnabled(bool rollEnabled)
        {
            GetComponent<Button>().enabled = rollEnabled;
            GetComponent<Image>().enabled = rollEnabled;
            _buttonText.text = rollEnabled ? "ROLL" : "";


        }

    }
}

