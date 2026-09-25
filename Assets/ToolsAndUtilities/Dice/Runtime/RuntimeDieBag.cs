using UnityEngine;
using System.Collections.Generic;

namespace DiceTools
{
    public enum DieBagType
    {
        DRAW_BAG,
        IN_HAND,
        DISCARD,
    }

    public class RuntimeDieBag
    {
        private string _displayName = string.Empty;
        private DieBagType _collectionType = DieBagType.DRAW_BAG;
        private List<RuntimeDie> _dice = new List<RuntimeDie>();


        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public DieBagType CollectionType { get { return _collectionType; } set { _collectionType = value; } }
        public List<RuntimeDie> Dice { get { return _dice; } set { _dice = value; } }
    }
}

