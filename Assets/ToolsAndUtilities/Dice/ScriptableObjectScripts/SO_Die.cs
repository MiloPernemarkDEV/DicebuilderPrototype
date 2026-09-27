using UnityEngine;
using System.Collections.Generic;

namespace DiceTools
{
    [CreateAssetMenu(fileName = "SO_Die", menuName = "Dice/Die")]
    public class SO_Die : ScriptableObject
    {
        [SerializeField] private string _displayName = string.Empty;
        [SerializeField] private List<SO_DieFace> _faces;
        [SerializeField] private List<string> _tagStrings = new List<string>();

        public RuntimeDie GetRuntimeDie()
        {
            RuntimeDie die = new RuntimeDie();
            die.DisplayName = _displayName;
            foreach (SO_DieFace face in _faces)
            {
                die.Faces.Add(face.GetRuntimeDieFace());
            }
            foreach (string tag in _tagStrings)
            {
                if (tag != string.Empty)
                {
                    die.TagStrings.Add(tag);
                }
            }
            return die;
        }
    }
}


