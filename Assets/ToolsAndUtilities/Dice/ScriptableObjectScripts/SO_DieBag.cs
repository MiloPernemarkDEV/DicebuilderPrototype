using UnityEngine;
using System.Collections.Generic;

namespace DiceTools
{
    [CreateAssetMenu(fileName = "SO_DieBag", menuName = "Dice/Dice Bag")]
    public sealed class SO_DieBag : ScriptableObject
    {
        [SerializeField] private string _displayName = string.Empty;
        [SerializeField] private DieBagType _collectionType = DieBagType.DRAW_BAG;
        [SerializeField] private List<SO_Die> _dice = new List<SO_Die>();


        public RuntimeDieBag GetRuntimeDieBag()
        {
            RuntimeDieBag dieBag = new RuntimeDieBag();
            //...
            dieBag.DisplayName = _displayName;
            dieBag.CollectionType = _collectionType;
            foreach(SO_Die die in _dice)
            {
                dieBag.Dice.Add(die.GetRuntimeDie());
            }
            return dieBag;
        }

    }
}


