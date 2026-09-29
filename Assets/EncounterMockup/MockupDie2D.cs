using System;
using Tweens;
using UnityEngine;
using UnityEngine.UIElements;

namespace EncounterMockup
{
    public class MockupDie2D : MonoBehaviour
    {
        [SerializeField] private float _startScale = 0.025f;
        [SerializeField] private float _maxScale = 0.6f;
        [SerializeField] private float _landScale = 0.4f;


        private void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        private void DoSizeUp(float durationSeconds)
        {
            Tween sizeUpTween = TweenService.GetFloatTween(
                gameObject,
                _startScale,
                _maxScale,
                durationSeconds * 0.5f,
                EnumTweenEase.SINE,
                EnumTweenDirection.OUT
                );
            sizeUpTween.OnValueUpdated += (value) =>
            {
                float scaleFloat = value.x;
                transform.localScale = new Vector3(scaleFloat, scaleFloat, scaleFloat);
            };
            sizeUpTween.OnFinished += () =>
            {
                DoSizeDown(durationSeconds);
            };

            sizeUpTween.StartTween();
        }
        private void DoSizeDown(float durationSeconds)
        {
            Tween sizeDownTween = TweenService.GetFloatTween(
                gameObject,
                _maxScale,
                _landScale,
                durationSeconds * 0.5f,
                EnumTweenEase.QUAD,
                EnumTweenDirection.IN
                );
            sizeDownTween.OnValueUpdated += (value) =>
            {
                float scaleFloat = value.x;
                transform.localScale = new Vector3(scaleFloat, scaleFloat, scaleFloat);
            };
            sizeDownTween.OnFinished += () =>
            {
                //...
            };
            sizeDownTween.StartTween();
        }

        public void DoRollingBehavior(float durationSeconds)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            DoSizeUp(durationSeconds);
        }
    }
}


