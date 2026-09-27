using UnityEngine;

namespace Encounter
{
    public class BlankCombatantObject : MonoBehaviour, ICombatantGameObject
    {
        public string SayHello()
        {
            return "Hi!";
        }
    }
}

