using UnityEngine;

namespace Encounter
{
    public class PlayerInHand_5Buttons : MonoBehaviour, IPlayerInHand
    {
        public void SetEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }
    }
}


