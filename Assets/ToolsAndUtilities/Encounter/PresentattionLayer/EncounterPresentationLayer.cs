using DiceTools;
using UnityEngine;

namespace Encounter
{
    public interface IPlayerInHand
    {
        public void SetEnabled(bool enabled);
        public void UpdateDice(RuntimeDieBag inHandDice);

        public void AnnouncePlayerInHandDieSelected();
        public void AnnouncePlayerInHandDieHovered();
    }

    public sealed class EncounterPresentationLayer : MonoBehaviour
    {
        [SerializeField] private GameObject _playerInHandObject;
        private IPlayerInHand _playerInHand;

        private void Awake()
        {
            _playerInHand = _playerInHandObject.GetComponent<IPlayerInHand>();
        }

        //=====
        // API
        //=====
        public IPlayerInHand PlayerInHand => _playerInHand;

    }
}


