using UnityEngine;

namespace Tweens
{
    public sealed class ScaleTweener : MonoBehaviour
    {
        public event System.Action ScaleTweenFinished;
        public void TweenObjectScale(
            GameObject obj,
            float endScale,
            float duration,
            EnumTweenEase easeType = EnumTweenEase.LINEAR,
            EnumTweenDirection easeDirection = EnumTweenDirection.IN_OUT
            )
        {
            Tween scaleTween = TweenService.GetFloatTween(
                obj,
                obj.transform.localScale.x,
                endScale,
                duration,
                easeType,
                easeDirection
                );
            scaleTween.OnFinished += () =>
            {
                ScaleTweenFinished?.Invoke();
            };
            scaleTween.OnValueUpdated += (value) =>
            {
                obj.transform.localScale = new Vector3(value.x, value.x, value.x);
            };
            scaleTween.StartTween();
        }
    }
}

