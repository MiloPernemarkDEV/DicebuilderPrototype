using UnityEngine;

namespace Tweens
{
    public sealed class PositionTweener : MonoBehaviour
    {
        

        public event System.Action PositionTweenFinished;
        public void TweenObjectToPosition(
            GameObject obj,
            Vector3 targetPosition,
            float duration,
            EnumTweenEase easeType = EnumTweenEase.LINEAR,
            EnumTweenDirection easeDirection = EnumTweenDirection.IN_OUT
            )
        {
            Tween posTween = TweenService.GetVector3Tween(
                obj,
                obj.transform.position,
                targetPosition,
                duration,
                easeType,
                easeDirection
                );
            posTween.OnFinished += () =>
            {
                PositionTweenFinished?.Invoke();
            };
            posTween.OnValueUpdated += (value) =>
            {
                obj.transform.position = value;
            };
            posTween.StartTween();
        }
    }
}

