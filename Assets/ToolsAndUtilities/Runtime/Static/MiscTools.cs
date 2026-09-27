using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace MiscTools
{
    public struct Transform2DTweaksStruct
    {
        public Vector3 PosTweak;
        public Vector3 AngleTweak;

        public Transform2DTweaksStruct(Vector3 _posTweak, Vector3 _angleTweak)
        {
            this.PosTweak = _posTweak;
            this.AngleTweak = _angleTweak;
        }
    }

    public static class MiscTransformTools
    {
        public static Transform2DTweaksStruct GetTransformTweak2D(float maxTweakPos, float maxTweakAngle)
        {
            Transform2DTweaksStruct tweaks = new Transform2DTweaksStruct(Vector3.zero, Vector3.zero);

            // ...

            float posXRando = UnityRandom.Range(-maxTweakPos, maxTweakPos);
            float posYRando = UnityRandom.Range(-maxTweakPos, maxTweakPos);
            Vector3 posAdjust = new Vector3(posXRando, posYRando, 0f);
            tweaks.PosTweak = posAdjust;

            float degreesRando = UnityRandom.Range(-maxTweakAngle, maxTweakAngle);
            Vector3 degreesAdjust = new Vector3(0, 0, degreesRando);
            tweaks.AngleTweak = degreesAdjust;

            return tweaks;
        }
    }
}