using UnityEngine;
using Tweens;
using DiceTools;
using System;

namespace DiceRolling
{
    public interface IVisibleThrowBehavior
    {
        public void DoThrowBehavior();
    }
    public sealed class VisibleThrowBehavior : MonoBehaviour, IVisibleThrowBehavior
    {
        private const float START_SCALE = 0f;
        private const float MAX_SCALE = 0.6f;
        private const float LAND_SCALE = 0.3f;
        private const float ROTATIONS = 2.5f;


        public void DoThrowBehavior()
        {
            float tweenDuration = DiceConstants.ROLL_DURATION;

            float sizeUpDuration = tweenDuration * MAX_SCALE;
            float sizeDownDuration = tweenDuration * (MAX_SCALE - LAND_SCALE);

            Tween sizeDownTween = TweenService.GetFloatTween(
                gameObject,
                MAX_SCALE,
                LAND_SCALE,
                sizeDownDuration,
                EnumTweenEase.SINE,
                EnumTweenDirection.IN
                );
            sizeDownTween.OnValueUpdated += (value) =>
            {
                transform.localScale = new Vector3(value.x, value.x, value.x);
            };

            Tween sizeUpTween = TweenService.GetFloatTween(
                gameObject,
                START_SCALE,
                MAX_SCALE,
                sizeUpDuration,
                EnumTweenEase.SINE,
                EnumTweenDirection.OUT
                );
            sizeUpTween.OnValueUpdated += (value) =>
            {
                transform.localScale = new Vector3(value.x, value.x, value.x);
            };

            sizeUpTween.OnFinished += () =>
            {
                sizeDownTween.StartTween();
            };

            Tween rotationTween = TweenService.GetFloatTween(
                gameObject,
                0f,
                360f * ROTATIONS,
                tweenDuration,
                EnumTweenEase.QUAD,
                EnumTweenDirection.OUT
                );
            rotationTween.OnValueUpdated += (value) =>
            {
                transform.rotation = Quaternion.Euler(0f, 0f, value.x);
            };

            sizeUpTween.StartTween();
            rotationTween.StartTween();
        }
    }
}


