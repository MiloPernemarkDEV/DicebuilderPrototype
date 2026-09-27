using UnityEngine;
using System.Collections.Generic;

namespace Encounter
{
    
    public sealed class RuntimeEncounterSetup
    {
        private RuntimeCombatantSetup _playerSetup = null;
        private List<RuntimeCombatantSetup> _enemySetups = new List<RuntimeCombatantSetup>();
        private RuntimeLocationSetup _locationSetup = null;

        private Dictionary<string, CombatantDice> _diceDict = new Dictionary<string, CombatantDice>();


        public RuntimeCombatantSetup PlayerSetup { get { return _playerSetup; } set { _playerSetup = value; } }
        public List<RuntimeCombatantSetup> EnemySetups { get { return _enemySetups; } set { _enemySetups = value; } }
        public RuntimeLocationSetup LocationSetup {  get { return _locationSetup; } set {_locationSetup = value; } }

    }
}

