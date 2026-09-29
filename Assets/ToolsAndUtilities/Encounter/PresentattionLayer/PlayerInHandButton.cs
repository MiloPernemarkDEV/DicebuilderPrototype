using DiceTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using EventChannels;

namespace Encounter
{
    public class PlayerInHandButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Sprite _blankSprite;
        [SerializeField] private SO_EventRuntimeDiePayload _dieSelectedEvent;
        [SerializeField] private SO_EventRuntimeDiePayload _dieUnselectedEvent;
        [SerializeField] private SO_EventIntPayload _selectedDiceUpdatedEvent;
        private RuntimeDie _dieData = null;

        private TMP_Text _hoverText = null;
        [SerializeField] private Image _selectedImage;
        private bool _selected = false;

        public RuntimeDie DieData => _dieData;

        private void Awake()
        {
            _hoverText = GetComponentInChildren<TMP_Text>();
            _hoverText.text = "";
            _selectedImage.enabled = _selected;
        }

        private void OnEnable()
        {
            _selectedDiceUpdatedEvent.OnEventTriggered += HandleSelectedDiceUpdated;
        }
        private void OnDisable()
        {
            _selectedDiceUpdatedEvent.OnEventTriggered -= HandleSelectedDiceUpdated;
        }

        private void HandleSelectedDiceUpdated(int selectedCount)
        {
            if (_selected) return;
            GetComponent<Button>().interactable = (selectedCount < EncounterConstants.MAX_SELECTED);
            
        }


        //=================

        public void SetDieData(RuntimeDie dieData)
        {
            _dieData = dieData;
            GetComponent<Image>().sprite = dieData.GetCurrentFace().ImageSprite;
            GetComponent<Button>().interactable = true;
        }
        public void ClearDieData()
        {
            _dieData = null;
            GetComponent<Image>().sprite = _blankSprite;
            GetComponent<Button>().interactable = false;
        }

        // pointer handler implementations
        public void OnPointerEnter(PointerEventData pData)
        {
            if (!GetComponent<Button>().interactable) return;
            if (_dieData == null) return;
            string dieName = _dieData.DisplayName;
            //Debug.Log($"### {name}: FOO {dieName}");
            _hoverText.text = dieName;
        }

        public void OnPointerExit(PointerEventData pData)
        {
            _hoverText.text = "";
        }

        public void OnPointerClick(PointerEventData pData)
        {
            if (!GetComponent<Button>().interactable) return;
            if (_dieData == null) return;
            string dieName = _dieData.DisplayName;
            //Debug.Log($"### {name}: BAR {dieName}");
            _selected = !_selected;
            _selectedImage.enabled = _selected;

            if (_selected)
            {
                _dieSelectedEvent.TriggerEvent(_dieData);
            }
            else
            {
                _dieUnselectedEvent.TriggerEvent(_dieData);
            }
        }



    }
}


