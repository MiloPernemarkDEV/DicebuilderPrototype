using UnityEngine;
using SimpleStateMachine;

namespace Encounter
{
    public sealed class DrawupStateBehavior : IStateBehaviors
    {
        public void DoStateEnteredBehavior(EncounterManager em)
        {
            Debug.Log($"### {em.name}: Drawup state entered");
        }
        public void DoStateExitedBehavior(EncounterManager em)
        {

        }
        public void DoUpdateBehavior(EncounterManager em)
        {

        }
    }
}


