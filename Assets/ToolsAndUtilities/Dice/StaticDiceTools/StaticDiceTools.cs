using UnityEngine;
using UnityRandom = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.Android;

namespace DiceTools
{

    

    public static class FaceAdjacencies
    {
        private static int GetRandomNextDeeSixIndex(int currentIdx)
        {
            List<List<int>> deeSixAdjacentFaces = new List<List<int>>()
            {
                new List<int> { 1,2,3,4 }, // 0
                new List<int> { 0,2,3,5 }, // 1
                new List<int> { 0,1,4,5 }, // 2
                new List<int> { 0,1,4,5 }, // 3
                new List<int> { 0,2,3,5 }, // 4
                new List<int> { 1,2,3,4 }, // 5
            };
            int randoInt = UnityRandom.Range(0, 4);
            return deeSixAdjacentFaces[currentIdx][randoInt];
        }

        private static int GetDefaultRandom(int currentIdx, int numberOfFaces)
        {
            List<int> candidateFaceIdxs = new List<int>();
            for (int i = 0; i < numberOfFaces; i++)
            {
                if (i != currentIdx)
                {
                    candidateFaceIdxs.Add(i);
                }
            }
            int randoInt = UnityRandom.Range(0, candidateFaceIdxs.Count);
            return candidateFaceIdxs[randoInt];

        }

        public static int GetRandomAdjacentFaceIndex(int currentFaceIndex, int numberOfFaces)
        {
            switch (numberOfFaces)
            {
                case 6:
                    return GetRandomNextDeeSixIndex(currentFaceIndex);

                default:
                    return GetDefaultRandom(currentFaceIndex, numberOfFaces); 
            }
        }
    }

    public static class StaticDiceTools
    {
        public static int GetRandomAdjacentFaceIndex(int numberOfFaces, int currentFaceIdx)
        {
            return -1;
        }
    }
}

