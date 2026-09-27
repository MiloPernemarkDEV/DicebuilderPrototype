using UnityEngine;

namespace Encounter
{
    public class EncounterTestRig : MonoBehaviour
    {
        [SerializeField] private SO_EncounterSetup _encounterSetup;
        public SO_EncounterSetup EncounterSetup => _encounterSetup;

    }
}
