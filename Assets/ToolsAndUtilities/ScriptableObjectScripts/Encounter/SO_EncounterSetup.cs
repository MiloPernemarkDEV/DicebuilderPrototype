using UnityEngine;
namespace Encounter

{
    [CreateAssetMenu(fileName = "SO_EncounterSetup", menuName = "Encounter/Encounter Setup")]
    public class SO_EncounterSetup : ScriptableObject
    {
        public RuntimeEncounterSetup GetRuntimeEncounterSetup()
        {
            RuntimeEncounterSetup setup = new RuntimeEncounterSetup();

            //...

            return setup;
        }
    }
}


