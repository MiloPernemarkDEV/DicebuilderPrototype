using Encounter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleStateMachine
{

    public interface IStateBehaviors
    {
        public void DoStateEnteredBehavior(EncounterManager em);
        public void DoStateExitedBehavior(EncounterManager em);
        public void DoUpdateBehavior(EncounterManager em);
    }

    public sealed class RuntimeSimpleStateMachine
    {
        private string _stateMachineName;
        private List<RuntimeSimpleState> _states;
        private RuntimeSimpleState _currentState;

        public event System.Action<string> StateEntered;
        public event System.Action<string> StateExited;

        public string CurrentStateName
        {
            get
            {
                return _currentState.StateName;
            }
        }

        public RuntimeSimpleStateMachine(string stateMachineName, List<RuntimeSimpleState> states)
        {
            _stateMachineName = stateMachineName;
            _states = states;
            _currentState = states[0];
        }

        public bool TryTakeTransition(string toStateName)
        {

            if (!_currentState.ToStates.Contains(toStateName))
            {
                Debug.LogWarning($"### {_stateMachineName}: State {_currentState.StateName} does not allow a transition to {toStateName}.");
                return false;
            }

            RuntimeSimpleState foundState = null;
            // get the state with this name
            foreach (RuntimeSimpleState state in _states)
            {
                if (state.StateName == toStateName)
                {
                    foundState = state;
                    break;
                }
            }
            if (foundState == null)
            {
                Debug.LogWarning($"### {_stateMachineName}: Contains no state with name {toStateName}");
                return false;
            }

            StateExited?.Invoke(_currentState.StateName);
            _currentState = foundState;
            StateEntered?.Invoke(_currentState.StateName);

            return true;
        }



    }
}