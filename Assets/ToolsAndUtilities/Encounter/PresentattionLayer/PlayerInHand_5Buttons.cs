using DiceTools;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Encounter
{
    public class PlayerInHand_5Buttons : MonoBehaviour, IPlayerInHand
    {
        [SerializeField] private List<Button> _diceButtons;
        private List<PlayerInHandButton> _playerInHandButtons = new List<PlayerInHandButton>();
        
        private List<RuntimeDie> _inHandDice = new List<RuntimeDie>();

        private void Awake()
        {
            _playerInHandButtons.Clear();
            foreach (Button butt in _diceButtons)
            {
                _playerInHandButtons.Add(butt.gameObject.GetComponent<PlayerInHandButton>());
            }
        }

        private void OnEnable()
        {
            //...
        }
        private void OnDisable()
        {
            //...
        }

        private void DisableAllButtons()
        {
            foreach(PlayerInHandButton butt in _playerInHandButtons)
            {
                butt.ClearDieData();
            }
        }

        // IPlayerInHand implementation
        public void AnnouncePlayerInHandDieSelected()
        {
            
        }
        public void AnnouncePlayerInHandDieHovered()
        {

        }
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
                PlayerInHandButton butt = _playerInHandButtons[i];
                butt.SetDieData(_inHandDice[i]);
            }
        }
    }
}