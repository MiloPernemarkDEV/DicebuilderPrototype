using DiceTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Encounter
{
    public class PlayerInHandButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private Sprite _blankSprite;
        private RuntimeDie _dieData = null;

        public RuntimeDie DieData => _dieData;

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
            if (_dieData == null) return;
            string dieName = _dieData.DisplayName;
            Debug.Log($"### {name}: FOO {dieName}");
        }

        public void OnPointerClick(PointerEventData pData)
        {
            if (_dieData == null) return;
            string dieName = _dieData.DisplayName;
            Debug.Log($"### {name}: BAR {dieName}");
        }



    }
}


