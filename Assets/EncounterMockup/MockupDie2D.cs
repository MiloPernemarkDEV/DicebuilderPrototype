using EventChannels;
using System;
using Tweens;
using UnityEngine;

namespace EncounterMockup
{
    public class MockupDie2D : MonoBehaviour
    {
        [SerializeField] private float _startScale = 0.025f;
        [SerializeField] private float _maxScale = 0.6f;
        [SerializeField] private float _landScale = 0.4f;
        [SerializeField] private SO_EventEmptyPayload _finishedEvent;

        private bool _isPlayer = false;


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

        private void DoSpin(float durationSeconds)
        {
            Tween spinTween = TweenService.GetFloatTween(
                gameObject,
                0f,
                720f,
                durationSeconds,
                EnumTweenEase.QUAD,
                EnumTweenDirection.OUT
                );
            spinTween.OnValueUpdated += (value) =>
            {
                //float currentRotZ = transform.rotation.eulerAngles.z;
                float nextZ = value.x;
                transform.rotation = Quaternion.Euler(0, 0, nextZ);

            };
            spinTween.StartTween();

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
                if (_isPlayer)
                {
                    _finishedEvent.TriggerEvent();
                }
            };
            sizeDownTween.StartTween();
        }

        public void DoRollingBehavior(float durationSeconds, bool isPlayer)
        {
            _isPlayer = isPlayer;
            GetComponent<SpriteRenderer>().enabled = true;
            DoSpin(durationSeconds);
            DoSizeUp(durationSeconds);
        }
    }
}


