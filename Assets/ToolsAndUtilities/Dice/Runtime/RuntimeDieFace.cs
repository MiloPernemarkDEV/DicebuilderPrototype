using System.Collections.Generic;
using UnityEngine;

namespace DiceTools
{
    public sealed class RuntimeDieFace
    {
        private int _integerValue = 0;
        private List<string> _tagStrings = new List<string>();
        private Sprite _imageSprite;

        public int IntegerValue { get { return _integerValue; } set { _integerValue = value; } }
        public List<string> TagStrings { get { return _tagStrings; }  set { _tagStrings = value; } }
        public Sprite ImageSprite { get { return _imageSprite; } set { _imageSprite = value; } }

    }
}

