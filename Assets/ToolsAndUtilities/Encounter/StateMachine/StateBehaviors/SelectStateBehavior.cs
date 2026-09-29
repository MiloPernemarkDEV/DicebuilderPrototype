using UnityEngine;
using SimpleStateMachine;
using System.Collections.Generic;
using DiceTools;

namespace Encounter
{
    public sealed class SelectStateBehavior : IStateBehaviors
    {
        private List<RuntimeDie> _selectedDice = new List<RuntimeDie>();


        public void DoStateEnteredBehavior(EncounterManager em)
        {
            Debug.Log($"### SelectStateBehavior: Hello");
            em.PresentationLayer.PlayerInHand.SetInteractable(true);
        }
        public void DoStateExitedBehavior(EncounterManager em)
        {

        }
        public void DoUpdateBehavior(EncounterManager em)
        {

        }

        public void HandleDieSelected(RuntimeDie d, EncounterManager em)
        {
            if (em.StateMachine.CurrentStateName != EncounterStates.SELECT)
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            Debug.Log($"### SelectStateBehaviors: Die selected -- {d.DisplayName}");
            if (_selectedDice.Contains(d))
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            _selectedDice.Add(d);
            Debug.Log($"### SelectStateBehaviors: selected count {_selectedDice.Count.ToString()}");
            em.SelectedDiceUpdatedEvent.TriggerEvent(_selectedDice.Count);
            em.PresentationLayer.RollCommandInterface.SetRollCommandEnabled(_selectedDice.Count >= EncounterConstants.MAX_SELECTED);
        }
        public void HandleDieUnselected(RuntimeDie d, EncounterManager em)
        {
            if (em.StateMachine.CurrentStateName != EncounterStates.SELECT)
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            Debug.Log($"### SelectStateBehaviors: Die unselected -- {d.DisplayName}");
            if (!_selectedDice.Contains(d))
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            _selectedDice.Remove(d);
            Debug.Log($"### SelectStateBehaviors: selected count {_selectedDice.Count.ToString()}");
            em.SelectedDiceUpdatedEvent?.TriggerEvent(_selectedDice.Count);
            em.PresentationLayer.RollCommandInterface.SetRollCommandEnabled(_selectedDice.Count >= EncounterConstants.MAX_SELECTED);
        }
    }
}


