using UnityEngine;
using SimpleStateMachine;

namespace Encounter
{
    public sealed class DrawupStateBehavior : IStateBehaviors
    {
        public void DoStateEnteredBehavior(EncounterManager em)
        {
            Debug.Log($"### {em.name}: Drawup state entered");

            CombatantDice playerDice = em.PlayerCombatant.CombatantDice;
            playerDice.DrawUp(em.MaxInHand);

            Debug.Log(EncounterTools.DebugCombatant(em, em.PlayerCombatant.RuntimeID, true));

            foreach (RuntimeCombatant enemyCombatant in em.EnemyCombatants)
            {
                enemyCombatant.CombatantDice.DrawUp(em.MaxInHand);
                Debug.Log(EncounterTools.DebugCombatant(em, enemyCombatant.RuntimeID, false));
            }

            // show the player's in-hand dice
            em.PresentationLayer.PlayerInHand.UpdateDice(em.PlayerCombatant.CombatantDice.Inhand);
            em.PresentationLayer.PlayerInHand.SetEnabled(true);

        }
        public void DoStateExitedBehavior(EncounterManager em)
        {

        }
        public void DoUpdateBehavior(EncounterManager em)
        {

        }
    }
}


