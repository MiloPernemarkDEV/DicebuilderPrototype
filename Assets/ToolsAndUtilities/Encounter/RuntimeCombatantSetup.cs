using DiceTools;
using UnityEngine;
namespace Encounter
{
    public sealed class RuntimeCombatantSetup
    {
        private string _displayName = "";
        private int _health = 20;
        private RuntimeDieBag _drawBag = null;
        private GameObject _combatantPrefab = null;
        

        public int Health { get { return _health; } set { _health = value; } }
        public RuntimeDieBag DrawBag { get { return _drawBag; } set { _drawBag = value; } }
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public GameObject CombatantPrefab { get { return _combatantPrefab; } set { _combatantPrefab = value; } }
    }
}