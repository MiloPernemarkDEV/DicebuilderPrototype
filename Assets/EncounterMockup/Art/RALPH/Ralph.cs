using EventChannels;
using SimpleStateMachine;
using UnityEngine;

namespace EncounterMockup
{
    public class Ralph : MonoBehaviour
    {
        [SerializeField] SO_SimpleStateMachine _ralphSM_SO;
        [SerializeField] SO_EventEmptyPayload _rollCommandEvent;
        private RuntimeSimpleStateMachine _ralphSM;

        private Animator _ralphAnimator;

        private void Awake()
        {
            _ralphAnimator = GetComponent<Animator>();
            _ralphSM = _ralphSM_SO.GetRuntimeSimpleStateMachine();
        }

        private void OnEnable()
        {
            _ralphSM.StateEntered += HandleStateEntered;
            _rollCommandEvent.OnEventTriggered += HandleRollCommandEVent;
        }
        private void OnDisable()
        {
            _ralphSM.StateEntered -= HandleStateEntered;
            _rollCommandEvent.OnEventTriggered -= HandleRollCommandEVent;
        }

        private void Start()
        {
            //_ralphAnimator.Play("RALPH_ATTACK");
            //Debug.Log(_ralphSM.CurrentStateName);

        }

        public void OnOneShotAnimFinished(string clipName)
        {
            Debug.Log(clipName);
            switch (clipName)
            {
                case "RALPH_ATTACK":
                    _ralphSM.TryTakeTransition("RALPH_IDLE");
                    return;
                case "RALPH_REACT":
                    _ralphSM.TryTakeTransition("RALPH_IDLE");
                    return;
                default:
                    return;
            }
        }

        private void HandleStateEntered(string stateName)
        {
            switch (stateName)
            {
                case "RALPH_IDLE":
                    _ralphAnimator.Play("RALPH_IDLE");
                    return;
                case "RALPH_ATTACK":
                    _ralphAnimator.Play("RALPH_ATTACK");
                    return;
                case "RALPH_REACT":
                    _ralphAnimator.Play("RALPH_REACT");
                    return;
                default:
                    return;
            }
        }

        private void HandleRollCommandEVent()
        {
            _ralphSM.TryTakeTransition("RALPH_ATTACK");
        }


    }
}


