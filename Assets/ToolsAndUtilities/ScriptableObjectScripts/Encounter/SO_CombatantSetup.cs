using DiceTools;
using UnityEngine;

namespace Encounter
{
    [CreateAssetMenu(fileName = "SO_CombatantSetup", menuName = "Encounter/Combatant Setup")]
    public class SO_CombatantSetup : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private SO_DieBag _drawBag;

        public RuntimeCombatantSetup GetRuntimeCombatantSetup()
        {
            RuntimeCombatantSetup setup = new RuntimeCombatantSetup();
            setup.Health = _health;
            setup.DrawBag = _drawBag.GetRuntimeDieBag();
            return setup;
        }
    }
}