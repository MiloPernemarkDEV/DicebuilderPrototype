using UnityEngine;
using DiceTools;
using MiscTools;
using System.Collections.Generic;


namespace Encounter
{
    public class DieSprite2D : MonoBehaviour, IDie2D
    {
        private Spinner _spinner = new Spinner();
        private RuntimeDie _runtimeDie = null;
        private SpriteRenderer _dieSpriteRenderer = null;
        private bool _isRolling;

        private Vector3 _startPos = Vector3.zero;

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
            _startPos = transform.position;
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
            RollingFinished?.Invoke();
        }
        private void HandleSpinnerUpdated()
        {
            if (_runtimeDie == null) return;
            _runtimeDie.SetNewCurrentFace();
            SetDieSprite();
        }

        private void SetDieSprite()
        {
            if (_runtimeDie == null) return;
            RuntimeDieFace currentFace = _runtimeDie.GetCurrentFace();
            _dieSpriteRenderer.sprite = currentFace.ImageSprite;
            TweakDieSprite();
        }

        private void TweakDieSprite()
        {
            Transform2DTweaksStruct tweak = MiscTransformTools.GetTransformTweak2D(0.5f, 10.0f);
            transform.position = tweak.PosTweak + _startPos;
            transform.rotation = Quaternion.Euler(0f, 0f, tweak.AngleTweak.z);
        }

        //=======================
        // IDie2D Implementation
        //=======================
        public event System.Action RollingFinished;

        public void SetRuntimeDie(RuntimeDie runtimeDie)
        {
            _runtimeDie = runtimeDie;
        }

        public void Roll()
        {
            if ( _runtimeDie == null) return;
            if (_isRolling) return;

            _spinner.Spin(gameObject, 4f, 50);
        }

        public bool GetIsRolling()
        {
            return _isRolling;
        }

        public DieFaceData GetCurrentDieFaceData()
        {
            RuntimeDieFace face = _runtimeDie.GetCurrentFace();
            int intValue = face.IntegerValue;
            List<string> tagStrings = face.TagStrings;
            return new DieFaceData(intValue, tagStrings);
        }
    }
}

