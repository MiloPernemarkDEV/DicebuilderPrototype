using UnityEngine;
using System.Collections.Generic;

namespace DiceTools
{
    [CreateAssetMenu(fileName = "SO_DieFace", menuName = "Dice/Die Face")]
    public class SO_DieFace : ScriptableObject
    {
        [SerializeField] private int _integerValue = 0;
        [SerializeField] private List<string> _tagStrings = new List<string>();
        [SerializeField] private Sprite _imageSprite;

        public RuntimeDieFace GetRuntimeDieFace()
        {
            RuntimeDieFace face = new RuntimeDieFace();
            face.IntegerValue = _integerValue;
            face.TagStrings = _tagStrings;
            face.ImageSprite = _imageSprite;
            return face;
        }

    }
}


