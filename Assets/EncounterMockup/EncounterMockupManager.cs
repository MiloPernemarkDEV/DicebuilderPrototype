using UnityEngine;
using EventChannels;
using DiceTools;
using Encounter;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace EncounterMockup
{
    public class EncounterMockupManager : MonoBehaviour
    {
        [SerializeField] private SO_EventEmptyPayload _rollCommandEvent;
        [SerializeField] private SO_EventEmptyPayload _opponentRollCommandEvent;
        [SerializeField] private GameObject _playerDiceHolder;
        [SerializeField] private GameObject _opponentDiceHolder;
        [SerializeField] private SO_DieBag _diceBagSO;
        [SerializeField] private SO_EventEmptyPayload _finishedEvent;

        private RuntimeDieBag _runtimeDieBag = null;

        private List<IDie2D> _playerDice = new List<IDie2D>();
        private List<IDie2D> _opponentDice = new List<IDie2D>();
        private List<MockupDie2D> _playerMockupDice = new List<MockupDie2D>();
        private List<MockupDie2D> _opponentMockupDice = new List<MockupDie2D>();

        private bool _diceAreRolling = false;

        bool aftermathTriggered = false;

        private bool triggered = false;

        private void Awake()
        {
            _runtimeDieBag = _diceBagSO.GetRuntimeDieBag();
        }

        private void OnEnable()
        {
            _rollCommandEvent.OnEventTriggered += HandleRollCommandEvent;
            _finishedEvent.OnEventTriggered += HandlePlayerRollingFinished;
            SetUpDice();
            ConnectRollingFinishedEvents();
        }
        private void OnDisable()
        {
            _rollCommandEvent.OnEventTriggered -= HandleRollCommandEvent;
            _finishedEvent.OnEventTriggered -= HandlePlayerRollingFinished;
            DisconnectRollingFinishedEvents();
        }

        private List<RuntimeDie> GetThreeRandomDiceFromBag()
        {
            List<RuntimeDie> outList = new List<RuntimeDie>();

            List<int> idxs = new List<int>();
            List<int> randoIdxs = new List<int>();
            for (int i = 0; i < _runtimeDieBag.Dice.Count; i++)
            {
                idxs.Add(i);
            }
            for (int i = 0; i < 3; i++)
            {
                int randoIdx = UnityRandom.Range(0, idxs.Count);
                randoIdxs.Add(idxs[randoIdx]);
                idxs.RemoveAt(randoIdx);
            }

            foreach(int randoIdx in randoIdxs)
            {
                RuntimeDie d = _runtimeDieBag.Dice[randoIdx];

                if (outList.Count < 3)
                {
                    outList.Add(d);
                }
            }
            return outList;
        }

        private void ConnectRollingFinishedEvents()
        {
            for (int i = 0; i < 3; i++)
            {
                _playerDice[i].RollingFinished += HandleRollingFinished;
            }
        }
        private void DisconnectRollingFinishedEvents()
        {
            for (int i = 0; i < 3; i++)
            {
                _playerDice[i].RollingFinished -= HandleRollingFinished;
            }
        }

        private void SetUpDice()
        {
            
            foreach (Transform child in _opponentDiceHolder.transform)
            {
                if (child.gameObject != _opponentDiceHolder.gameObject)
                {
                    _opponentDice.Add(child.GetComponent<IDie2D>());
                    _opponentMockupDice.Add(child.GetComponent<MockupDie2D>());
                }
            }

            foreach (Transform child in _playerDiceHolder.transform)
            {
                if (child.gameObject != _playerDiceHolder.gameObject)
                {
                    _playerDice.Add(child.GetComponent<IDie2D>());
                    _playerMockupDice.Add(child.GetComponent<MockupDie2D>());
                }

            }

            List<RuntimeDie> _playerDiceData = GetThreeRandomDiceFromBag();
            List<RuntimeDie> _opponentDiceData = GetThreeRandomDiceFromBag();

            if (_playerDice.Count != _playerDiceData.Count || _opponentDice.Count != _opponentDiceData.Count || _playerDice.Count != 3 || _opponentDice.Count != 3)
            {
                Debug.LogError("Something weird happened");
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                _playerDice[i].SetRuntimeDie(_playerDiceData[i]);
                _opponentDice[i].SetRuntimeDie(_opponentDiceData[i]);
            }


        }

        private void RollDice()
        {
            for (int i= 0; i < 3; i++)
            {
                _playerDice[i].Roll();
                _playerMockupDice[i].DoRollingBehavior(.75f, true);
                //_opponentDice[i].Roll();
                //_opponentMockupDice[i].DoRollingBehavior(0.75f);
            }
            //StartCoroutine(WaitThenTriggerOpponentRoll(1.5f));
        }

        private void HandleRollCommandEvent()
        {
            if (_diceAreRolling) return;
            _diceAreRolling = true;
            RollDice();
        }

        private void HandleRollingFinished()
        {
            if (aftermathTriggered) return;
            aftermathTriggered = true;
            StartCoroutine(WaitThenTriggerAftermath(3.0f));
        }

        private System.Collections.IEnumerator WaitThenTriggerOpponentRoll(float wait)
        {
            yield return new WaitForSecondsRealtime(wait);

            for (int i = 0; i < 3; i++)
            {
                _opponentDice[i].Roll();
                _opponentMockupDice[i].DoRollingBehavior(.75f, false);
                //_playerDice[i].Roll();
                //_playerMockupDice[i].DoRollingBehavior(0.75f);
            }
            _opponentRollCommandEvent.TriggerEvent();
        }

        private void HandlePlayerRollingFinished()
        {
            if (triggered) return;
            triggered = true;

            for (int i = 0; i < 3; i++)
            {
                _opponentDice[i].Roll();
                _opponentMockupDice[i].DoRollingBehavior(.75f, false);
                //_playerDice[i].Roll();
                //_playerMockupDice[i].DoRollingBehavior(0.75f);
            }
            _opponentRollCommandEvent.TriggerEvent();
        }

        private System.Collections.IEnumerator WaitThenTriggerAftermath(float wait)
        {
            yield return new WaitForSecondsRealtime(wait);

            List<GameObject> playerDice = new List<GameObject>();
            List<GameObject> oppoDice = new List<GameObject>();

            
            foreach(Transform child in _opponentDiceHolder.transform)
            {
                if(child.gameObject != _opponentDiceHolder.gameObject)
                {
                    oppoDice.Add(child.gameObject);
                }
            }

            foreach (Transform child in _playerDiceHolder.transform)
            {
                if (child.gameObject != _playerDiceHolder.gameObject)
                {
                    playerDice.Add(child.gameObject);
                }
            }

            GetComponent<AftermathController>().DoAftermath(playerDice, oppoDice);
        }


    }
}



