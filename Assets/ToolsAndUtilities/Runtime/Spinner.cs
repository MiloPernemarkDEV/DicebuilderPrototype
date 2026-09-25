using Tweens;
using UnityEngine;
namespace DiceTools
{
    public sealed class Spinner
    {

        private bool _isSpinning;
        public event System.Action SpinningStarted;
        public event System.Action SpinningFinished;
        public event System.Action SpinnerUpdated;


        public void Spin(GameObject owner, float spinDuration, int steps)
        {
            if (_isSpinning)
            {
                return;
            }
            _isSpinning = true;

            if (spinDuration < 0.1f || steps <= 0)
            {
                // throw an error
                return;
            }

            float stepAmount = 10f / steps;
            Tween spinTween = TweenService.GetFloatTween(
                owner,
                0f,
                10f,
                spinDuration,
                EnumTweenEase.QUART,
                EnumTweenDirection.OUT
                );

            spinTween.OnFinished += () =>
            {
                _isSpinning = false;
                SpinningFinished?.Invoke();
            };

            float _accumulator = 0f;
            spinTween.OnValueUpdated += (value) =>
            {
                float progress = value.x;
                if (progress >= stepAmount + _accumulator)
                {
                    _accumulator = progress;
                    SpinnerUpdated?.Invoke();
                }
            };
            spinTween.StartTween();
            SpinningStarted?.Invoke();
        }

    }
}

