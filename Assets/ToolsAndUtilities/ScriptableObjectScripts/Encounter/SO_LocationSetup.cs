using UnityEngine;
namespace Encounter
{
    [CreateAssetMenu(fileName = "SO_LocationSetup", menuName = "Encounter/Location Setup")]
    public class SO_LocationSetup : ScriptableObject
    {
        public RuntimeLocationSetup GetRuntimeLocationSetup()
        {
            RuntimeLocationSetup loc = new RuntimeLocationSetup();

            //...

            return loc;
        }
    }
}