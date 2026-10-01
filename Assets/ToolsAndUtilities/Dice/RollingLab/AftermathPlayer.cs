using TMPro;
using UnityEngine;
using Tweens;

namespace DiceRolling
{
    public sealed class AftermathPlayer : MonoBehaviour
    {
        private const float TWEEN_DURATION = .5f;
        [SerializeField] private TMP_Text _playerAttackText;
        [SerializeField] private TMP_Text _playerEvasionText;
        [SerializeField] private TMP_Text _opponentAttackText;
        [SerializeField] private TMP_Text _opponentEvasionText;

        public event System.Action PlayerAttackTweenFinished;
        public event System.Action OpponentAttackTweenFinished;

        public void PlayerAttack()
        {
            PositionTweener posTweener = _playerAttackText.GetComponent<PositionTweener>();
            posTweener.TweenObjectToPosition(
                _playerAttackText.gameObject,
                _opponentEvasionText.transform.position,
                TWEEN_DURATION,
                EnumTweenEase.QUAD,
                EnumTweenDirection.IN
                );
            posTweener.PositionTweenFinished += () =>
            {
                PlayerAttackTweenFinished?.Invoke();
            };
        }
        public void OpponentAttack()
        {
            PositionTweener posTweener = _opponentAttackText.GetComponent<PositionTweener>();
            posTweener.TweenObjectToPosition(
                _opponentAttackText.gameObject,
                _playerEvasionText.transform.position,
                TWEEN_DURATION,
                EnumTweenEase.QUAD,
                EnumTweenDirection.IN
                );
            posTweener.PositionTweenFinished += () =>
            {
                OpponentAttackTweenFinished?.Invoke();
            };
        }
    }
}

