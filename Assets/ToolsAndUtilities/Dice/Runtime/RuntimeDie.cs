using System.Collections.Generic;
using UnityEngine;

namespace DiceTools
{
    public sealed class RuntimeDie
    {
        private string _displayName = string.Empty;
        private List<RuntimeDieFace> _faces = new List<RuntimeDieFace>();
        private List<string> _tagStrings = new List<string>();

        private int _currentFaceIndex = 0;

        private bool _isRolling = false;


        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public List<RuntimeDieFace> Faces { get { return _faces; } set { _faces = value; } }
        public List<string> TagStrings {  get { return _tagStrings; } set {_tagStrings = value; } }

        public RuntimeDieFace GetCurrentFace()
        {
            return _faces[_currentFaceIndex];
        }

        public void SetNewCurrentFace()
        {
            int nextFaceIdx = FaceAdjacencies.GetRandomAdjacentFaceIndex(_currentFaceIndex, _faces.Count);
            nextFaceIdx = nextFaceIdx % _faces.Count;
            _currentFaceIndex = nextFaceIdx;
        }

    }
}

