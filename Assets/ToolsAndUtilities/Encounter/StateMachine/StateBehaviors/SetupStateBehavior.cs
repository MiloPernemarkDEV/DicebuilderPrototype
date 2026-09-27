using UnityEngine;
using SimpleStateMachine;

namespace Encounter
{
    public sealed class SetupStateBehavior : IStateBehaviors
    {
        public void DoStateEnteredBehavior(EncounterManager em)
        {
            Debug.Log($"### {em.name}: Setting things up...");

            // turn off the test rig if the EncounterService Singleton is present
            bool enableTestRig = (EncounterService.Instance == null) ? true : false;
            em.TestRig.enabled = enableTestRig;

            if (em.TestRig.enabled)
            {
                // get the setup data from the test rig
            }
            else
            {
                // get the setup data from the singleton
            }

        }
        public void DoStateExitedBehavior(EncounterManager em)
        {

        }
        public void DoUpdateBehavior(EncounterManager em)
        {

        }
    }
}


