using UnityEngine;
using DiceTools;
using System.Collections.Generic;
using TMPro;
using Encounter;
using EventChannels;
using EncounterMockup;
using Tweens;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DiceRolling
{




    public sealed class DiceRollingTester : MonoBehaviour
    {
        private const float WAIT_BEFORE_OPPONENT_ROLL = 1.0f;
        private const float WAIT_BEFORE_OUTCOME = 1.0f;
        private const float OUTCOME_TWEEN_DURATION = 0.25f;
        private const float WAIT_BEFORE_AFTERMATH = 1.5f;

        private const string ATTACK = "ACTION.ATTACK.LIGHT";
        private const string EVASION = "ACTION.EVASION";

        [SerializeField] private DieSpriteHolder _playerDieSpriteHolder = null;
        [SerializeField] private DieSpriteHolder _opponentDieSpriteHolder = null;
        [SerializeField] private SO_DieBag _dieBag = null;
        [SerializeField] private List<TMP_Text> _resultTexts = new List<TMP_Text>();
        [SerializeField] private List<TMP_Text> _opponentResultTexts = new List<TMP_Text>();

        [SerializeField] private TMP_Text _playerAttackOutcomeText = null;
        [SerializeField] private TMP_Text _playerEvasionOutcomeText = null;
        [SerializeField] private TMP_Text _opponentAttackOutcomeText = null;
        [SerializeField] private TMP_Text _opponentEvasionOutcomeText = null;
        [SerializeField] private SO_EventEmptyPayload _rollCommandEvent = null;
        [SerializeField] private Ralph _ralph = null;
        [SerializeField] private HitMissText _playerHitMissText;
        [SerializeField] private HitMissText _opponentHitMissText;

        [SerializeField] private Button _reloadButton;
        private RuntimeDieBag _runtimeDieBag = null;

        private List<GameObject> _playerAttackDieObjs = new List<GameObject>();
        private List<GameObject> _opponentAttackDieObjs = new List<GameObject>();
        private List<GameObject> _playerEvasionDieObjs = new List<GameObject>();
        private List<GameObject> _opponentEvasionDieObjs = new List<GameObject>();

        private int _playerAttackTotal = 0;
        private int _playerEvasionTotal = 0;
        private int _opponentAttackTotal = 0;
        private int _opponentEvasionTotal = 0;

        private void Awake()
        {
            _runtimeDieBag = _dieBag.GetRuntimeDieBag();
        }

        private void OnEnable()
        {
            _playerDieSpriteHolder.AllDiceFinishedRolling   += HandlePlayerDiceFinishedRolling;
            _opponentDieSpriteHolder.AllDiceFinishedRolling += HandleOpponentDiceFinishedRolling;
            _rollCommandEvent.OnEventTriggered              += HandleRollCommandEvent;
            _reloadButton.onClick.AddListener(HandleReloadPressed);
        }
        private void OnDisable()
        {
            _playerDieSpriteHolder.AllDiceFinishedRolling   -= HandlePlayerDiceFinishedRolling;
            _opponentDieSpriteHolder.AllDiceFinishedRolling -= HandleOpponentDiceFinishedRolling;
            _rollCommandEvent.OnEventTriggered              -= HandleRollCommandEvent;
            _reloadButton.onClick.RemoveAllListeners();
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
            _opponentDieSpriteHolder.SetDiceData(selectedRuntimeDice);

        }

        private void HandleReloadPressed()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

            int playerAttackTotal = 0;
            int playerEvasionTotal = 0;

            List<GameObject> dieObjects = _playerDieSpriteHolder.GetDieObjects();
            for (int i = 0; i < dieObjects.Count; i++)
            {
                DieFaceData currentFace = dieObjects[i].GetComponent<IDie2D>().GetCurrentDieFaceData();
                int faceValue = currentFace.IntegerValue;
                
                //_resultTexts[i].text = $"{faceValue.ToString()}";

                string tag = currentFace.TagStrings[0];
                if (tag == ATTACK)
                {
                    playerAttackTotal += faceValue;
                    _playerAttackDieObjs.Add(dieObjects[i]);
                }
                else if (tag == EVASION)
                {
                    playerEvasionTotal += faceValue;
                    _playerEvasionDieObjs.Add(dieObjects[i]);
                }
            }
            _playerAttackOutcomeText.text = $"Attack: {playerAttackTotal}";
            _playerEvasionOutcomeText.text = $"Evasion: {playerEvasionTotal}";

            _playerAttackTotal = playerAttackTotal;
            _playerEvasionTotal = playerEvasionTotal;
            Debug.Log($"Player Attack Total: {_playerAttackTotal}, Player Evasion Total: {_playerEvasionTotal}");

            StartCoroutine(WaitThenRollOpponentDice(WAIT_BEFORE_OPPONENT_ROLL));
        }

        private void HandleOpponentDiceFinishedRolling()
        {
            if (_opponentResultTexts.Count != _opponentDieSpriteHolder.GetDieObjects().Count)
            {
                Debug.LogError("Mismatch between opponent result texts count and die objects count.");
                return;
            }

            int opponentAttackTotal = 0;
            int opponentEvasionTotal = 0;

            List<GameObject> dieObjects = _opponentDieSpriteHolder.GetDieObjects();
            for (int i = 0; i < dieObjects.Count; i++)
            {
                DieFaceData currentFace = dieObjects[i].GetComponent<IDie2D>().GetCurrentDieFaceData();
                int faceValue = currentFace.IntegerValue;

                //_opponentResultTexts[i].text = $"{faceValue.ToString()}";

                string tag = currentFace.TagStrings[0];
                if (tag == ATTACK)
                {
                    opponentAttackTotal += faceValue;
                    _opponentAttackDieObjs.Add(dieObjects[i]);
                }
                else if (tag == EVASION)
                {
                    opponentEvasionTotal += faceValue;
                    _opponentEvasionDieObjs.Add(dieObjects[i]);
                }
            }
            _opponentAttackOutcomeText.text = $"Attack: {opponentAttackTotal}";
            _opponentEvasionOutcomeText.text = $"Evasion: {opponentEvasionTotal}";

            _opponentEvasionTotal = opponentEvasionTotal;
            _opponentAttackTotal = opponentAttackTotal;

            // Start the outcome routime after a short delay
            StartCoroutine(WaitThenShowOutcome(WAIT_BEFORE_OUTCOME));

        }

        private void SetOutcomeTextsVisible(bool visible)
        {
            _playerAttackOutcomeText.gameObject.SetActive(visible);
            _playerEvasionOutcomeText.gameObject.SetActive(visible);
            _opponentAttackOutcomeText.gameObject.SetActive(visible);
            _opponentEvasionOutcomeText.gameObject.SetActive(visible);
        }

        private void HandleOutcome()
        {
            foreach(GameObject obj in _playerAttackDieObjs)
            {
                if (obj.GetComponent<PositionTweener>() != null)
                {
                    obj.GetComponent<PositionTweener>().TweenObjectToPosition(
                        obj,
                        _playerAttackOutcomeText.transform.position,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
                if (obj.GetComponent<ScaleTweener>() != null)
                {
                    obj.GetComponent<ScaleTweener>().TweenObjectScale(
                        obj,
                        0f,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
            }

            foreach (GameObject obj in _playerEvasionDieObjs)
            {
                if (obj.GetComponent<PositionTweener>() != null)
                {
                    obj.GetComponent<PositionTweener>().TweenObjectToPosition(
                        obj,
                        _playerEvasionOutcomeText.transform.position,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
                if (obj.GetComponent<ScaleTweener>() != null)
                {
                    obj.GetComponent<ScaleTweener>().TweenObjectScale(
                        obj,
                        0f,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
            }

            foreach (GameObject obj in _opponentAttackDieObjs)
            {
                if (obj.GetComponent<PositionTweener>() != null)
                {
                    obj.GetComponent<PositionTweener>().TweenObjectToPosition(
                        obj,
                        _opponentAttackOutcomeText.transform.position,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
                if (obj.GetComponent<ScaleTweener>() != null)
                {
                    obj.GetComponent<ScaleTweener>().TweenObjectScale(
                        obj,
                        0f,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
            }

            foreach (GameObject obj in _opponentEvasionDieObjs)
            {
                if (obj.GetComponent<PositionTweener>() != null)
                {
                    obj.GetComponent<PositionTweener>().TweenObjectToPosition(
                        obj,
                        _opponentEvasionOutcomeText.transform.position,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
                if (obj.GetComponent<ScaleTweener>() != null)
                {
                    obj.GetComponent<ScaleTweener>().TweenObjectScale(
                        obj,
                        0f,
                        OUTCOME_TWEEN_DURATION,
                        EnumTweenEase.QUAD,
                        EnumTweenDirection.IN
                        );
                }
            }
            SetOutcomeTextsVisible(true);

            StartCoroutine(WaitThenAftermath(WAIT_BEFORE_AFTERMATH));

        }

        private System.Collections.IEnumerator WaitThenRollOpponentDice(float wait)
        {
            yield return new WaitForSeconds(wait);
            Debug.Log($"### {name}: Rolling opponent dice");
            _ralph.RalphSM.TryTakeTransition("RALPH_ATTACK");
            _opponentDieSpriteHolder.SetDiceVisible(true);
            _opponentDieSpriteHolder.RollDice();
        }
        private System.Collections.IEnumerator WaitThenShowOutcome(float wait)
        {
            yield return new WaitForSeconds(wait);
            HandleOutcome();
        }

        private System.Collections.IEnumerator WaitThenAftermath(float wait)
        {
            yield return new WaitForSeconds(wait);

            AftermathPlayer ap = GetComponent<AftermathPlayer>();
            ap.PlayerAttack();
            ap.PlayerAttackTweenFinished += () =>
            {
                _opponentEvasionOutcomeText.text = "";
                _playerAttackOutcomeText.text = "";

                if (_playerAttackTotal >= _opponentEvasionTotal && _playerAttackTotal > 0)
                {
                    _opponentHitMissText.GetComponent<TMP_Text>().text = "HIT!";
                    _opponentHitMissText.GetComponent<TMP_Text>().color = Color.red;
                    _ralph.RalphSM.TryTakeTransition("RALPH_REACT");
                }
                else
                {
                    _opponentHitMissText.GetComponent<TMP_Text>().text = "MISS!";
                    _opponentHitMissText.GetComponent<TMP_Text>().color = Color.gold;
                }
                _opponentHitMissText.PlayHit();


                ap.OpponentAttack();
                ap.OpponentAttackTweenFinished += () =>
                {
                    _playerEvasionOutcomeText.text = "";
                    _opponentAttackOutcomeText.text = "";

                    //Debug.Log($"Opponent attack: {_opponentAttackTotal}, Player evasion: {_playerEvasionTotal}");

                    if (_opponentAttackTotal >= _playerEvasionTotal && _opponentAttackTotal > 0)
                    {
                        _playerHitMissText.GetComponent<TMP_Text>().text = "HIT!";
                        _playerHitMissText.GetComponent<TMP_Text>().color = Color.red;
                        
                    }
                    else
                    {
                        _playerHitMissText.GetComponent<TMP_Text>().text = "MISS!";
                        _playerHitMissText.GetComponent<TMP_Text>().color = Color.gold;
                    }
                    _playerHitMissText.PlayHit();
                };
            };
        }
    }
}

