using SimpleStateMachine;
using UnityEngine;

namespace EncounterMockup
{
    public class Ralph : MonoBehaviour
    {
        [SerializeField] SO_SimpleStateMachine _ralphSM_SO;
        private RuntimeSimpleStateMachine _ralphSM;

        private Animator _ralphAnimator;

        private void Awake()
        {
            _ralphAnimator = GetComponent<Animator>();
            _ralphSM = _ralphSM_SO.GetRuntimeSimpleStateMachine();
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
                    _ralphAnimator.Play("RALPH_REACT");
                    return;
                case "RALPH_REACT":
                    _ralphAnimator.Play("RALPH_ATTACK");
                    return;
                default:
                    return;
            }

        }


    }
}


