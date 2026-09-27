using DiceTools;
using UnityEngine;

namespace Encounter
{
    public interface ICombatantGameObject
    {
        public string SayHello();
    }

    public class RuntimeCombatant
    {
        private string _displayName = "";
        private int _health = -1;
        //private ICombatantGameObject _combatantGameObject = null;
        private string _runtimeID = "";
        private CombatantDice _combatantDice = null;
        private bool _isPlayer = false;

        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public int Health { get { return _health; } set { _health = value; } }
        public string RuntimeID { get { return _runtimeID; } set { _runtimeID = value; } }
        public bool IsPlayer { get { return _isPlayer; } set { _isPlayer = value; } }

        public CombatantDice CombatantDice { get { return _combatantDice; } }

        public void SetCombatantDice (RuntimeDieBag drawBag)
        {
            _combatantDice = new CombatantDice(drawBag);
        }

    }
}

