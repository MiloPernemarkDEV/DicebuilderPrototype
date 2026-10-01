using UnityEngine;
using Tweens;
using TMPro;
using DiceTools;
using Encounter;
using System.Collections.Generic;

namespace EncounterMockup
{

    public enum CombatantType { PLAYER, OPPONENT };
    public enum ActionType { ATTACK, EVASION };

    public class AftermathController : MonoBehaviour
    {
        private const float TWEEN_DURATION = 0.5f;


        [SerializeField] private GameObject _resultsTextHolder;
        [SerializeField] private TMP_Text _playerAttackText;
        [SerializeField] private TMP_Text _playerEvasionText;
        [SerializeField] private TMP_Text _opponentAttackText;
        [SerializeField] private TMP_Text _opponentEvasionText;

        private Vector3 _playerAttackTweenToPos;
        private Vector3 _playerEvasionTweenToPos;
        private Vector3 _opponentAttackTweenToPos;
        private Vector3 _opponentEvasionTweenToPos;

        private bool textShown = false;

        private void Awake()
        {
            _playerAttackTweenToPos    = _playerAttackText.transform.position;
            _playerEvasionTweenToPos   = _playerEvasionText.transform.position;
            _opponentAttackTweenToPos  = _opponentAttackText.transform.position;
            _opponentEvasionTweenToPos = _opponentEvasionText.transform.position;
            
            _resultsTextHolder.gameObject.SetActive(false);
            _playerAttackText.gameObject.SetActive(false);
            _playerEvasionText.gameObject.SetActive(false);
            _opponentAttackText.gameObject.SetActive(false);
            _opponentEvasionText.gameObject.SetActive(false);

        }

        private void DoAttackAftermath(List<GameObject> dice, CombatantType combatantType, ActionType actionType)
        {

            Vector3 destPos = Vector3.zero;
            string label = string.Empty;
            GameObject textObject = null;

            switch (combatantType)
            {
                case CombatantType.PLAYER:
                    if (actionType == ActionType.ATTACK)
                    {
                        destPos = _playerAttackTweenToPos;
                        label = "Attack: ";
                        textObject = _playerAttackText.gameObject;
                        break;
                    }
                    else
                    {
                        destPos = _playerEvasionTweenToPos;
                        label = "Evasion: ";
                        textObject = _playerEvasionText.gameObject;
                        break;
                    }
                case CombatantType.OPPONENT:
                    if (actionType == ActionType.ATTACK)
                    {
                        destPos = _opponentAttackTweenToPos;
                        label = "Attack: ";
                        textObject = _opponentAttackText.gameObject;
                        break;
                    }
                    else
                    {
                        destPos = _opponentEvasionTweenToPos;
                        label = "Evasion: ";
                        textObject = _opponentEvasionText.gameObject;
                        break;
                    }
                default:
                    return;
            }

            int totalValue = 0;

            if (dice.Count <= 0)
            {
                textObject.GetComponent<TMP_Text>().text = $"{label}{totalValue}";
                return;
            }

            

            foreach (GameObject dObj in dice)
            {
                Vector3 startPos = dObj.transform.position;
                float startScale = dObj.transform.localScale.x;
                float endScale = 0f;

                Tween diePosTween = TweenService.GetVector3Tween(
                    gameObject,
                    startPos,
                    destPos,
                    TWEEN_DURATION,
                    EnumTweenEase.QUART,
                    EnumTweenDirection.IN
                    );
                diePosTween.OnValueUpdated += (value) =>
                {
                    Vector3 newPos = new Vector3(value.x, value.y, value.z);
                    dObj.transform.position = newPos;
                };
                diePosTween.OnFinished += () =>
                {
                    if (textShown) return;
                    ShowText();
                };

                Tween dieScaleTween = TweenService.GetFloatTween(
                    gameObject,
                    startScale,
                    endScale,
                    TWEEN_DURATION,
                    EnumTweenEase.QUART,
                    EnumTweenDirection.IN
                    );
                dieScaleTween.OnValueUpdated += (value) =>
                {
                    Vector3 newScale = new Vector3(value.x, value.x, value.x);
                    dObj.transform.localScale = newScale;
                };
                diePosTween.StartTween();
                dieScaleTween.StartTween();

                int thisValue = dObj.GetComponent<IDie2D>().GetCurrentDieFaceData().IntegerValue;

                totalValue += thisValue;
            }

            textObject.GetComponent<TMP_Text>().text = $"{label}{totalValue.ToString()}";
        }

        private void ShowText()
        {
            if(textShown) return;
            _playerAttackText.gameObject.SetActive(true);
            _playerEvasionText.gameObject.SetActive(true);
            _opponentAttackText.gameObject.SetActive(true);
            _opponentEvasionText.gameObject.SetActive(true);
        }

        public void DoAftermath(List<GameObject> playerDice, List<GameObject> opponentDice)
        {
            _resultsTextHolder.gameObject.SetActive(true);

            List<GameObject> playerAttackDice    = new List<GameObject>();
            List<GameObject> playerEvasionDice   = new List<GameObject>();
            List<GameObject> opponentAttackDice  = new List<GameObject>();
            List<GameObject> opponentEvasionDice = new List<GameObject>();

            
            foreach (GameObject od in opponentDice)
            {
                IDie2D rd = od.GetComponent<IDie2D>();
                DieFaceData face = rd.GetCurrentDieFaceData();
                switch (face.TagStrings[0])
                {
                    case "ACTION.ATTACK.LIGHT":
                        opponentAttackDice.Add(od);
                        break;
                    case "ACTION.EVASION":
                        opponentEvasionDice.Add(od);
                        break;
                    default:
                        break;
                }
            }

            foreach (GameObject pd in playerDice)
            {
                IDie2D rd = pd.GetComponent<IDie2D>();
                DieFaceData face = rd.GetCurrentDieFaceData();
                switch (face.TagStrings[0])
                {
                    case "ACTION.ATTACK.LIGHT":
                        playerAttackDice.Add(pd);
                        break;
                    case "ACTION.EVASION":
                        playerEvasionDice.Add(pd);
                        break;
                    default:
                        break;
                }
            }

            DoAttackAftermath(playerAttackDice, CombatantType.PLAYER, ActionType.ATTACK);
            DoAttackAftermath(playerEvasionDice, CombatantType.PLAYER, ActionType.EVASION);
            DoAttackAftermath(opponentAttackDice, CombatantType.OPPONENT, ActionType.ATTACK);
            DoAttackAftermath(opponentEvasionDice, CombatantType.OPPONENT, ActionType.EVASION);
        }
    }
}

