using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    [CreateAssetMenu(fileName = "SO_SimpleStateMachine", menuName = "Simple State Machine/Simple State Machine")]
    public class SO_SimpleStateMachine : ScriptableObject
    {
        [SerializeField] private string _stateMachineName = "My state machine";
        [SerializeField, Tooltip("First item is initial state.")] private List<SO_SimpleState> _states;

        [HideInInspector] public string StateMachineName { get { return _stateMachineName; } }
        [HideInInspector] public List<SO_SimpleState> States { get { return _states; } }

        
        public RuntimeSimpleStateMachine GetRuntimeSimpleStateMachine()
        {
            List<RuntimeSimpleState> runtimeStates = new List<RuntimeSimpleState>();
            foreach (SO_SimpleState state in _states)
            {
                runtimeStates.Add(state.GetRuntimeSimpleState());
            }
            return new RuntimeSimpleStateMachine(_stateMachineName, runtimeStates);
        }
        

    }
}