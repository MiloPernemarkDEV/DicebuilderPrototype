using UnityEngine;
using DiceTools;
using System.Collections.Generic;
using TMPro;
using Encounter;
using EventChannels;
using EncounterMockup;

namespace DiceRolling
{
    public sealed class DiceRollingTester : MonoBehaviour
    {
        private const float WAIT_BEFORE_OPPONENT_ROLL = 1.0f;

        [SerializeField] private DieSpriteHolder _playerDieSpriteHolder = null;
        [SerializeField] private DieSpriteHolder _opponentDiceHolder = null;
        [SerializeField] private SO_DieBag _dieBag = null;
        [SerializeField] private List<TMP_Text> _resultTexts = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _opponentResultTexts = new List<TMP_Text>();

        [SerializeField] private TMP_Text _playerAttackOutcomeText = null;
        [SerializeField] private TMP_Text _playerEvasionOutcomeText = null;
        [SerializeField] private TMP_Text _opponentAttackOutcomeText = null;
        [SerializeField] private TMP_Text _opponentEvasionOutcomeText = null;
        [SerializeField] private SO_EventEmptyPayload _rollCommandEvent = null;
        [SerializeField] private Ralph _ralph = null;
        private RuntimeDieBag _runtimeDieBag = null;

        private void Awake()
        {
            _runtimeDieBag = _dieBag.GetRuntimeDieBag();
        }

        private void OnEnable()
        {
            _playerDieSpriteHolder.AllDiceFinishedRolling += HandlePlayerDiceFinishedRolling;
            _opponentDiceHolder.AllDiceFinishedRolling    += HandleOpponentDiceFinishedRolling;
            _rollCommandEvent.OnEventTriggered            += HandleRollCommandEvent;
        }
        private void OnDisable()
        {
            _playerDieSpriteHolder.AllDiceFinishedRolling -= HandlePlayerDiceFinishedRolling;
            _opponentDiceHolder.AllDiceFinishedRolling    -= HandleOpponentDiceFinishedRolling;
            _rollCommandEvent.OnEventTriggered            -= HandleRollCommandEvent;
        }

        private void Start()
        {
            ClearResultTexts();
            SetOutcomeTextsVisible(false);
            List<int> selectedDieIdxs = _runtimeDieBag.GetRandomDieIdxs(3);
            List<RuntimeDie> selectedRuntimeDice = new List<RuntimeDie>();
            foreach (int idx in selectedDieIdxs)
            {
                selectedRuntimeDice.Add(_runtimeDieBag.Dice[idx]);
            }
            _playerDieSpriteHolder.SetDiceData(selectedRuntimeDice);

            selectedDieIdxs.Clear();
            selectedRuntimeDice.Clear();
            selectedDieIdxs = _runtimeDieBag.GetRandomDieIdxs(3);
            foreach (int idx in selectedDieIdxs)
            {
                selectedRuntimeDice.Add(_runtimeDieBag.Dice[idx]);
            }
            _opponentDiceHolder.SetDiceData(selectedRuntimeDice);

        }

        private void HandleRollCommandEvent()
        {
            _playerDieSpriteHolder.SetDiceVisible(true);
            _playerDieSpriteHolder.RollDice();
        }

        private void ClearResultTexts()
        {
            foreach (TMP_Text text in _resultTexts)
            {
                text.text = string.Empty;
            }
        }

        private void HandlePlayerDiceFinishedRolling()
        {
            if (_resultTexts.Count != _playerDieSpriteHolder.GetDieObjects().Count)
            {
                Debug.LogError("Mismatch between result texts count and die objects count.");
                return;
            }
            List<GameObject> dieObjects = _playerDieSpriteHolder.GetDieObjects();
            for (int i = 0; i < dieObjects.Count; i++)
            {
                DieFaceData currentFace = dieObjects[i].GetComponent<IDie2D>().GetCurrentDieFaceData();
                int faceValue = currentFace.IntegerValue;
                _resultTexts[i].text = $"{faceValue.ToString()}";
            }
            StartCoroutine(WaitThenRollOpponentDice(WAIT_BEFORE_OPPONENT_ROLL));
        }

        private void HandleOpponentDiceFinishedRolling()
        {
            if (_opponentResultTexts.Count != _opponentDiceHolder.GetDieObjects().Count)
            {
                Debug.LogError("Mismatch between opponent result texts count and die objects count.");
                return;
            }
            List<GameObject> dieObjects = _opponentDiceHolder.GetDieObjects();
            for (int i = 0; i < dieObjects.Count; i++)
            {
                DieFaceData currentFace = dieObjects[i].GetComponent<IDie2D>().GetCurrentDieFaceData();
                int faceValue = currentFace.IntegerValue;
                _opponentResultTexts[i].text = $"{faceValue.ToString()}";
            }
        }

        private void SetOutcomeTextsVisible(bool visible)
        {
            _playerAttackOutcomeText.gameObject.SetActive(visible);
            _playerEvasionOutcomeText.gameObject.SetActive(visible);
            _opponentAttackOutcomeText.gameObject.SetActive(visible);
            _opponentEvasionOutcomeText.gameObject.SetActive(visible);
        }

        private System.Collections.IEnumerator WaitThenRollOpponentDice(float wait)
        {
            yield return new WaitForSeconds(wait);
            Debug.Log($"### {name}: Rolling opponent dice");
            _ralph.RalphSM.TryTakeTransition("RALPH_ATTACK");
            _opponentDiceHolder.SetDiceVisible(true);
            _opponentDiceHolder.RollDice();

        }
    }
}

