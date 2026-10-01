using UnityEngine;
using SimpleStateMachine;
using System.Collections.Generic;
using DiceTools;
using System.Linq;

namespace Encounter
{
    public sealed class SelectStateBehavior : IStateBehaviors
    {
        private List<string> _selectedDiceIDs = new List<string>();
        private Dictionary<string, List<string>> _enemySelectedDiceIDs = new Dictionary<string, List<string>>();

        public void DoStateEnteredBehavior(EncounterManager em)
        {
            Debug.Log($"### SelectStateBehavior: Hello");

            // have each enemy select their dices
            foreach (RuntimeCombatant enemyCombatant in em.EnemyCombatants)
            {
                List<string> selectedEnemyDiceIDs = new List<string>();
                List<int> selectedDieIdxs = em.DiceSelector.MakeSelection(enemyCombatant.CombatantDice.Inhand, em);
                foreach (int i in selectedDieIdxs)
                {
                    selectedEnemyDiceIDs.Add(enemyCombatant.CombatantDice.Inhand.Dice[i].RuntimeID);
                }
                _enemySelectedDiceIDs[enemyCombatant.RuntimeID] = selectedEnemyDiceIDs;
            }


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

            string dID = d.RuntimeID;
            if (_selectedDiceIDs.Contains(dID))
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            _selectedDiceIDs.Add(dID);
            Debug.Log($"### SelectStateBehaviors: selected count {_selectedDiceIDs.Count.ToString()}");
            em.SelectedDiceUpdatedEvent.TriggerEvent(_selectedDiceIDs.Count);
            bool readyToRoll = _selectedDiceIDs.Count >= EncounterConstants.MAX_SELECTED;
            em.PresentationLayer.RollCommandInterface.SetRollCommandEnabled(readyToRoll);

        }
        public void HandleDieUnselected(RuntimeDie d, EncounterManager em)
        {
            if (em.StateMachine.CurrentStateName != EncounterStates.SELECT)
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            Debug.Log($"### SelectStateBehaviors: Die unselected -- {d.DisplayName}");

            string dID = d.RuntimeID;
            if (!_selectedDiceIDs.Contains(dID))
            {
                Debug.LogError($"### SelectStateBehaviors: Something weird happened");
                return;
            }

            _selectedDiceIDs.Remove(dID);
            Debug.Log($"### SelectStateBehaviors: selected count {_selectedDiceIDs.Count.ToString()}");
            em.SelectedDiceUpdatedEvent?.TriggerEvent(_selectedDiceIDs.Count);
            bool readyToRoll = _selectedDiceIDs.Count >= EncounterConstants.MAX_SELECTED;
            em.PresentationLayer.RollCommandInterface.SetRollCommandEnabled(readyToRoll);
        }

        public void HandleRollCommand(EncounterManager em)
        {
            em.PlayerCombatant.CombatantDice.InHandToInPlay(_selectedDiceIDs);
            Debug.Log(EncounterTools.DebugCombatant(em, em.PlayerCombatant.RuntimeID, true));


            foreach (string id in _enemySelectedDiceIDs.Keys.ToList())
            {
                RuntimeCombatant combatant = em.EnemyCombatants.Find(c => c.RuntimeID == id);
                if (combatant != null)
                {
                    combatant.CombatantDice.InHandToInPlay(_enemySelectedDiceIDs[id]);
                    Debug.Log(EncounterTools.DebugCombatant(em, combatant.RuntimeID, false));
                }
            }

            em.PresentationLayer.PlayerInHand.SetInteractable(false);
            em.StateMachine.TryTakeTransition(EncounterStates.ROLLING);
        }
    }
}


