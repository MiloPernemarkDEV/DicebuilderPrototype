using DiceTools;
using EventChannels;
using UnityEngine;
using UnityEngine.UI;

namespace Encounter
{
    public interface IPlayerInHand
    {
        public void SetEnabled(bool enabled);
        public void SetInteractable(bool interactable);
        public void UpdateDice(RuntimeDieBag inHandDice);

        public void AnnouncePlayerInHandDieSelected();
        public void AnnouncePlayerInHandDieHovered();
    }

    public interface IRollCommandInterface
    {
        public void SetRollCommandEnabled(bool rollEnabled);
    }

    public sealed class EncounterPresentationLayer : MonoBehaviour
    {
        [SerializeField] private GameObject _playerInHandObject;
        [SerializeField] private GameObject _rollCommandObject;
        //[SerializeField] private SO_EventEmptyPayload _rollCommandEvent;


        private IPlayerInHand _playerInHand;
        private IRollCommandInterface _rollCommandInterface;

        private void Awake()
        {
            _playerInHand = _playerInHandObject.GetComponent<IPlayerInHand>();
            _rollCommandInterface = _rollCommandObject.GetComponent<IRollCommandInterface>();
        }

        private void OnEnable()
        {
            //_rollCommandEvent.OnEventTriggered += HandleRollCommandEvent;
        }
        private void OnDisable()
        {
            //_rollCommandEvent.OnEventTriggered -= HandleRollCommandEvent;
        }

        private void Start()
        {
            _rollCommandInterface.SetRollCommandEnabled(false);
        }

        

        //=====
        // API
        //=====
        public IPlayerInHand PlayerInHand => _playerInHand;
        public IRollCommandInterface RollCommandInterface => _rollCommandInterface;

    }
}


