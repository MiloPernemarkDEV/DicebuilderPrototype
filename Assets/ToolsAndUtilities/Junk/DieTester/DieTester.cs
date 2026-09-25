using UnityEngine;
using DiceTools;
using Encounter;
using UnityEngine.UI;
using TMPro;

namespace DieTesting
{
    public class DieTester : MonoBehaviour
    {
        [SerializeField] private SO_Die _dieSO;
        [SerializeField] private GameObject _die2DObject;
        [SerializeField] private Button _rollButton;
        [SerializeField] private TMP_Text _resultText;

        private IDie2D _die2D;

        private void Awake()
        {
            _die2D = _die2DObject.GetComponent<IDie2D>();
            
        }

        private void OnEnable()
        {
            _die2D.SetRuntimeDie(_dieSO.GetRuntimeDie());
            _rollButton.onClick.AddListener(RollDie);
            _die2D.RollingFinished += HandleRollingFinished;

        }

        private void OnDisable()
        {
            _rollButton.onClick.RemoveAllListeners();
            _die2D.RollingFinished -= HandleRollingFinished;
        }

        private void RollDie()
        {
            _rollButton.interactable = false;
            _resultText.text = "Rolling...";
            _die2D.Roll();
        }

        private void HandleRollingFinished()
        {
            _rollButton.interactable = true;
            DieFaceData faceData = _die2D.GetCurrentDieFaceData();

            string resultString = "";

            if (faceData.TagStrings.Count> 0)
            {
                resultString = $"Value: {faceData.IntegerValue}\nTag: {faceData.TagStrings[0]}";
            }
            _resultText.text = resultString;
        }

    }

}
