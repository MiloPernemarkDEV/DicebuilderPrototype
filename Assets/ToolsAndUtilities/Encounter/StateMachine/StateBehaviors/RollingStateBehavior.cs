using UnityEngine;
using SimpleStateMachine;

namespace Encounter
{
    public sealed class RollingStateBehavior : IStateBehaviors
    {
        public void DoStateEnteredBehavior(EncounterManager em)
        {
            Debug.Log("### RollingStateBehavior: Hello");

            //...
        }
        public void DoStateExitedBehavior(EncounterManager em)
        {

        }
        public void DoUpdateBehavior(EncounterManager em)
        {

        }
    }
}


