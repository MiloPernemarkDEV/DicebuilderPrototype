using UnityEngine;
using DiceTools;


namespace Encounter
{
    public class DieSprite2D : MonoBehaviour, IDie2D
    {
        private Spinner _spinner = new Spinner();
        private RuntimeDie _runtimeDie = null;
        private SpriteRenderer _dieSpriteRenderer = null;
        private bool _isRolling;

        private void Awake()
        {
            _dieSpriteRenderer = GetComponent<SpriteRenderer>();
            if (_dieSpriteRenderer == null)
            {
                // throw an error
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _spinner.SpinningStarted  += HandleSpinnerStarted;
            _spinner.SpinningFinished += HandleSpinnerFinished;
            _spinner.SpinnerUpdated   += HandleSpinnerUpdated;
        }

        private void OnDisable()
        {
            _spinner.SpinningStarted  -= HandleSpinnerStarted;
            _spinner.SpinningFinished -= HandleSpinnerFinished;
            _spinner.SpinnerUpdated   -= HandleSpinnerUpdated;
        }

        private void Start()
        {
            SetDieSprite();
        }

        private void HandleSpinnerStarted()
        {
            if (_runtimeDie == null) return;
        }
        private void HandleSpinnerFinished()
        {
            if (_runtimeDie == null) return;
        }
        private void HandleSpinnerUpdated()
        {
            if (_runtimeDie == null) return;
        }

        private void SetDieSprite()
        {
            if (_runtimeDie == null) return;
            RuntimeDieFace currentFace = _runtimeDie.GetCurrentFace();
            _dieSpriteRenderer.sprite = currentFace.ImageSprite;
        }

        //=======================
        // IDie2D Implementation
        //=======================
        public void SetRuntimeDie(RuntimeDie runtimeDie)
        {
            _runtimeDie = runtimeDie;
        }

        public void Roll()
        {
            if ( _runtimeDie == null) return;
            if (_isRolling) return;



        }
        public bool GetIsRolling()
        {
            return _isRolling;
        }
    }
}

