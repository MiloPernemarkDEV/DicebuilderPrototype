using TMPro;
using UnityEngine;
using Tweens;

namespace DiceRolling
{
    public sealed class HitMissText : MonoBehaviour
    {
        private const float UP_AMOUNT = 3f;
        private const float TWEEN_DURATION = 1.0f;

        private TMP_Text _hmText;
        private PositionTweener _posTweener;
        private TextAlphaTweener _alphaTweener;
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            _endPosition = new Vector3(_startPosition.x, _startPosition.y + UP_AMOUNT, _startPosition.z);
            _hmText = GetComponent<TMP_Text>();
            _posTweener = GetComponent<PositionTweener>();
            _alphaTweener = GetComponent<TextAlphaTweener>();
        }

        private void Start()
        {
            
        }

        public void PlayHit()
        {
            //_hmText.color = Color.red;
            //_hmText.text = "HIT!";
            _posTweener.TweenObjectToPosition(
                gameObject,
                _endPosition,
                TWEEN_DURATION,
                EnumTweenEase.QUAD,
                EnumTweenDirection.IN
                );
            _alphaTweener.TweenTextAlpha(
                _hmText,
                0f,
                TWEEN_DURATION,
                EnumTweenEase.QUAD,
                EnumTweenDirection.OUT
                );
        }


    }
}

