using DiceTools;
using UnityEngine;
namespace Encounter
{

    public interface ICombatantGameObject
    {
        //...
    }

    public sealed class RuntimeCombatantSetup
    {
        private string _displayName = "";
        private string _combatantID = "";
        private int _health = 20;
        private RuntimeDieBag _drawBag = null;
        private GameObject _combatantPrefab = null;
        

        public int Health { get { return _health; } set { _health = value; } }
        public RuntimeDieBag DrawBag { get { return _drawBag; } set { _drawBag = value; } }
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public string CombatantID { get { return _combatantID; } set { _combatantID = value; } }
        public GameObject CombatantPrefab { get { return _combatantPrefab; } set { _combatantPrefab = value; } }
    }
}