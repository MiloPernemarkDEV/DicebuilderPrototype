using UnityEngine;
using SimpleStateMachine;

namespace Encounter
{
    public sealed class SetupStateBehavior : IStateBehaviors
    {
        public void DoStateEnteredBehavior(EncounterManager em)
        {

            // turn off the test rig if the EncounterService Singleton is present
            bool enableTestRig = (EncounterService.Instance == null) ? true : false;
            em.TestRig.enabled = enableTestRig;

            if (em.TestRig.enabled)
            {
                // get the setup data from the test rig
                em.EncounterSetup = em.TestRig.EncounterSetup.GetRuntimeEncounterSetup();

                // create a RuntimeCombatant for the player
                em.PlayerCombatant = new RuntimeCombatant();
                em.PlayerCombatant.DisplayName = em.EncounterSetup.PlayerSetup.DisplayName;
                em.PlayerCombatant.Health = em.EncounterSetup.PlayerSetup.Health;
                em.PlayerCombatant.RuntimeID = System.Guid.NewGuid().ToString();
                em.PlayerCombatant.IsPlayer = true;
                em.PlayerCombatant.SetCombatantDice(em.EncounterSetup.PlayerSetup.DrawBag);

                em.InstantiateCombatantObject(em.PlayerCombatant.RuntimeID, em.EncounterSetup.PlayerSetup.CombatantPrefab);

                // create a RuntimeCOmbatant for each enemy
                foreach (RuntimeCombatantSetup enemySetup in em.EncounterSetup.EnemySetups)
                {
                    RuntimeCombatant rc = new RuntimeCombatant();
                    rc.DisplayName = enemySetup.DisplayName;
                    rc.Health = enemySetup.Health;
                    rc.RuntimeID = System.Guid.NewGuid().ToString();
                    rc.IsPlayer = false;
                    rc.SetCombatantDice(enemySetup.DrawBag);
                    em.EnemyCombatants.Add(rc);

                    em.InstantiateCombatantObject(rc.RuntimeID, enemySetup.CombatantPrefab);
                }

                // set up the location

                //DebugSetup(em);

                em.StateMachine.TryTakeTransition(EncounterStates.DRAWUP);

            }
            else
            {
                return; // temp until the singleton is done.
                // get the setup data from the singleton
            }
        }
        public void DoStateExitedBehavior(EncounterManager em)
        {

        }
        public void DoUpdateBehavior(EncounterManager em)
        {

        }

        private void DebugSetup(EncounterManager em)
        {
            string playerID = em.PlayerCombatant.RuntimeID;
            string hiStr = em.CombatantObjects[playerID].SayHello();

            string playerStr = "";
            playerStr += $"Player Name: {em.PlayerCombatant.DisplayName}\n";
            int drawBagCount = em.PlayerCombatant.CombatantDice.DrawBag.Dice.Count;
            int inHandCount = em.PlayerCombatant.CombatantDice.Inhand.Dice.Count;
            int inPlayCount = em.PlayerCombatant.CombatantDice.InPlay.Dice.Count;
            int discardBagCount = em.PlayerCombatant.CombatantDice.DiscardBag.Dice.Count;
            playerStr += $"{drawBagCount} -- {inHandCount} -- {inPlayCount} -- {discardBagCount} -- {hiStr}";
            Debug.Log(playerStr);
        }

        

    }
}


