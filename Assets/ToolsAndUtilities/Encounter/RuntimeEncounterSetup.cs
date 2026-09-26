using UnityEngine;
using System.Collections.Generic;

namespace Encounter
{
    public sealed class RuntimeEncounterSetup
    {
        private RuntimeCombatantSetup _playerSetup = null;
        private List<RuntimeCombatantSetup> _enemySetups = new List<RuntimeCombatantSetup>();
    }
}

