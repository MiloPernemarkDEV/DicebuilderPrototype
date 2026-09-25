using UnityEngine;
using DiceTools;
using Encounter;

namespace DieTesting
{
    public class DieTester : MonoBehaviour
    {
        [SerializeField] private SO_Die _dieSO;
        [SerializeField] private GameObject _die2DObject;

        private IDie2D _die2D;

        private void Awake()
        {
            _die2D = _die2DObject.GetComponent<IDie2D>();
        }

        private void OnEnable()
        {
            _die2D.SetRuntimeDie(_dieSO.GetRuntimeDie());
        }

        public void RollDie()
        {
            _die2D.Roll();
        }

    }

}
