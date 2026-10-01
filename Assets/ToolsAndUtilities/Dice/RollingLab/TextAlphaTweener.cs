using TMPro;
using UnityEngine;

namespace Tweens
{
    public sealed class TextAlphaTweener : MonoBehaviour
    {
        public event System.Action TextAlphaTweenFinished;
        public void TweenTextAlpha(
            TMP_Text t,
            float targetAphpa,
            float duration,
            EnumTweenEase easeType = EnumTweenEase.LINEAR,
            EnumTweenDirection easeDirection = EnumTweenDirection.IN_OUT
            )
        {
            Tween alphaTween = TweenService.GetFloatTween(
                t.gameObject,
                t.color.a,
                targetAphpa,
                duration,
                easeType,
                easeDirection
                );
            alphaTween.OnFinished += () =>
            {
                TextAlphaTweenFinished?.Invoke();
            };
            alphaTween.OnValueUpdated += (value) =>
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, value.x);
            };
            alphaTween.StartTween();
        }
    }
}


