using UnityEngine;
using System.Collections.Generic;
using DiceTools;

namespace Encounter
{
    public sealed class DieFaceData
    {
        private int _integerValue = 0;
        private List<string> _tagStrings = new List<string>();
        public int IntegerValue { get { return _integerValue; } }
        public IReadOnlyList<string> TagStrings { get { return _tagStrings; } }

        public DieFaceData(int integerValue, List<string> tagStrings)
        {
            _integerValue = integerValue;
            _tagStrings = tagStrings;
        }

    }

    public interface IDie2D
    {
        public void SetRuntimeDie(RuntimeDie runtimeDie);
        public void Roll();
        public bool GetIsRolling();

        public DieFaceData GetCurrentDieFaceData();

        public event System.Action RollingFinished;
    }

}
