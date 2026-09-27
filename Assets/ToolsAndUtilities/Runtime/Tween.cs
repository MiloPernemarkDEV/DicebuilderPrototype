using UnityEngine;
namespace Tweens
{
    /*
    //===============
    // USAGE EXAMPLE
    //===============
    
    private void TestTween()
    {
        float testFloat = 0f;
        Tween testTween = TweenService.GetFloatTween(gameObject, 0f, 1f, 5f, EnumTweenEase.CUBIC, EnumTweenDirection.IN);
        testTween.OnValueUpdated += (value) =>
        {
            testFloat = value.x;
            Debug.Log(testFloat);
        };
        testTween.StartTween();
    }
     */

    public sealed class Tween : MonoBehaviour, ITween
    {
        public System.Action<Vector3> OnValueUpdated;
        public System.Action OnFinished;

        private GameObject owner;
        private Vector3 startValue;
        private Vector3 endValue;
        private float duration;
        private float elapsed;
        private bool running;
        private bool autodestruct;
        private System.Func<float, float> easeFunc;

        public void Initialize
            (
                GameObject _owner,
                Vector3 _startValue,
                Vector3 _endValue,
                float _duration,
                EnumTweenEase _ease,
                EnumTweenDirection _direction,
                bool _autodestruct = true
            )
        {
            if (_duration < TweenService.MINIMUM_DURATION)
            {
                Debug.LogWarning($"Duration is too low. The duration will be {TweenService.MINIMUM_DURATION}");
                _duration = TweenService.MINIMUM_DURATION;
            }
            owner = _owner;
            startValue = _startValue;
            endValue = _endValue;
            duration = _duration;
            easeFunc = TweenService.GetEaseFunction(_ease, _direction);
            autodestruct = _autodestruct;
        }
        public void StartTween()
        {
            running = true;
        }
        public void PauseTween()
        {
            running = false;
        }
        public void SetRepeat(bool value)
        {
            // TODO
        }
        public void ResetTween()
        {
            // TODO
        }

        private void FixedUpdate()
        {
            if (owner == null) { Destroy(gameObject); return; }
            if (!running) { return; }

            elapsed += Time.deltaTime;
            float completeRatio = Mathf.Clamp01(elapsed / duration);
            float eased = easeFunc(completeRatio);
            Vector3 current = Vector3.Lerp(startValue, endValue, eased);
            OnValueUpdated?.Invoke(current);

            if (completeRatio >= 1f)
            {
                running = false;
                OnFinished?.Invoke();
                if (autodestruct) { Destroy(gameObject); }
            }
        }
        private void OnDestroy()
        {
            Debug.Log("GOODBYE CRUEL WORLD");
        }
    }
}