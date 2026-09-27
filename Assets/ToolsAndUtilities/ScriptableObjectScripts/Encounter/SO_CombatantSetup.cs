using DiceTools;
using UnityEngine;

namespace Encounter
{
    [CreateAssetMenu(fileName = "SO_CombatantSetup", menuName = "Encounter/Combatant Setup")]
    public class SO_CombatantSetup : ScriptableObject
    {
        [SerializeField] private string _displayName = "";
        [SerializeField] private string _combatantID = "";
        [SerializeField] private int _health;
        [SerializeField] private SO_DieBag _drawBag;
        [SerializeField] private GameObject _combatantPrefab;

        public RuntimeCombatantSetup GetRuntimeCombatantSetup()
        {
            RuntimeCombatantSetup setup = new RuntimeCombatantSetup();
            setup.DisplayName = _displayName;
            setup.CombatantID = _combatantID;
            setup.Health = _health;
            setup.DrawBag = _drawBag.GetRuntimeDieBag();
            setup.CombatantPrefab = _combatantPrefab;
            return setup;
        }
    }
}