using UnityEngine;
using System.Collections.Generic;

namespace SimpleStateMachine
{
    public class RuntimeSimpleState
    {
        private string _stateName;
        private List<string> _toStates;

        public string StateName { get { return _stateName; } }
        public IReadOnlyList<string> ToStates { get { return _toStates; } }

        public RuntimeSimpleState(string stateName, List<string> toStates)
        {
            _stateName = stateName;
            _toStates = toStates;
        }
    }
}