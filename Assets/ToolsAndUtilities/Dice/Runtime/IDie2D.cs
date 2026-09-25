using UnityEngine;
using DiceTools;

namespace Encounter
{
    public interface IDie2D
    {
        public void SetRuntimeDie(RuntimeDie runtimeDie);
        public void Roll();
        public bool GetIsRolling();

        public event System.Action RollingFinished;
    }

}
