using DiceTools;
using UnityEngine;
namespace Encounter
{
    public sealed class RuntimeCombatantSetup
    {
        private int _health = 20;
        private RuntimeDieBag _drawBag = null;

        public int Health { get { return _health; } set { _health = value; } }
        public RuntimeDieBag DrawBag { get { return _drawBag; } set { _drawBag = value; } }
    }
}