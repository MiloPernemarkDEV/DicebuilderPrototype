using UnityEngine;
using System.Collections.Generic;

namespace Encounter

{
    [CreateAssetMenu(fileName = "SO_EncounterSetup", menuName = "Encounter/Encounter Setup")]
    public class SO_EncounterSetup : ScriptableObject
    {
        [SerializeField] private SO_CombatantSetup _playerSetup;
        [SerializeField] private List<SO_CombatantSetup> _enemySetups;
        [SerializeField] private SO_LocationSetup _locationSetup;

        public RuntimeEncounterSetup GetRuntimeEncounterSetup()
        {
            RuntimeEncounterSetup setup = new RuntimeEncounterSetup();

            setup.LocationSetup = _locationSetup.GetRuntimeLocationSetup();
            setup.PlayerSetup = _playerSetup.GetRuntimeCombatantSetup();
            foreach(SO_CombatantSetup enemySetup in _enemySetups)
            {
                setup.EnemySetups.Add(enemySetup.GetRuntimeCombatantSetup());
            }

            return setup;
        }
    }
}