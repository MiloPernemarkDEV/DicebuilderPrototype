using DiceTools;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Encounter
{
    public class PlayerInHand_5Buttons : MonoBehaviour, IPlayerInHand
    {
        [SerializeField] List<Button> _diceButtons;
        private List<RuntimeDie> _inHandDice = new List<RuntimeDie>();

        private void OnEnable()
        {
            /*
            for (int i = 0; i < _diceButtons.Count; i++)
            {
                _diceButtons[i].onClick.AddListener(() => HandleButtonPressed(i));
            }
            */
            // ^^this doesn't work like I think it does. I hooked up the
            //     buttons in the editor instead for now
        }
        private void OnDisable()
        {
            /*
            foreach (Button butt in _diceButtons)
            {
                butt.onClick.RemoveAllListeners();
            }
            */
        }

        public void HandleButtonPressed(int buttonIdx)
        {
            Button thisButton = _diceButtons[buttonIdx];
            RuntimeDie thisDie = _inHandDice[buttonIdx];
            Debug.Log(thisDie.DisplayName);
            Debug.Log(thisDie.RuntimeID);
        }

        private void DisableAllButtons()
        {
            foreach(Button butt in _diceButtons)
            {
                butt.interactable = false;
            }
        }

        // IPlayerInHand implementation
        public event System.Action<string> InHandDieSelected;
        public void SetEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        public void UpdateDice(RuntimeDieBag inHandDice)
        {
            _inHandDice.Clear();
            DisableAllButtons();
            foreach (RuntimeDie d in inHandDice.Dice)
            {
                _inHandDice.Add(d);
            }
            for (int i = 0; i < _inHandDice.Count; i++)
            {
                Button butt = _diceButtons[i];
                Image buttImage = butt.GetComponent<Image>();
                buttImage.sprite = _inHandDice[i].GetCurrentFace().ImageSprite;
                butt.interactable = true;
            }
            

        }
    }
}