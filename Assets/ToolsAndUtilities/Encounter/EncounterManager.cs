using UnityEngine;
using SimpleStateMachine;

namespace Encounter
{
    public static class EncounterStates
    {
        public const string SETUP       = "SETUP";
        public const string DRAWUP      = "DRAWUP";
        public const string SELECT      = "SELECT";
        public const string ROLLING     = "ROLLING";
        public const string RESOLUTION  = "RESOLUTION";
        public const string AFTERMATH   = "AFTERMATH";
        public const string PLAYER_DEAD = "PLAYER_DEAD";
        public const string PLAYER_WIN  = "PLAYER_WIN";
    }

    public sealed class EncounterManager : MonoBehaviour
    {
        [SerializeField] private SO_SimpleStateMachine _stateMachineSO;
        private RuntimeSimpleStateMachine _stateMachine = null;

        private IStateBehaviors _setupBehaviors;
        private IStateBehaviors _drawupBehaviors;
        private IStateBehaviors _selectBehaviors;
        private IStateBehaviors _rollingBehaviors;
        private IStateBehaviors _resolutionBehaviors;
        private IStateBehaviors _aftermathBehaviors;
        private IStateBehaviors _playerDeadBehaviors;
        private IStateBehaviors _playerWinBehaviors;

        private void Awake()
        {
            if ( _stateMachineSO != null)
            {
                _stateMachine = _stateMachineSO.GetRuntimeSimpleStateMachine();
            }
            _setupBehaviors      = new SetupStateBehavior();
            _selectBehaviors     = new SelectStateBehavior();
            _drawupBehaviors     = new DrawupStateBehavior();
            _rollingBehaviors    = new RollingStateBehavior();
            _resolutionBehaviors = new ResolutionStateBehavior();
            _aftermathBehaviors  = new AftermathStateBehavior();
            _playerDeadBehaviors = new PlayerDeadStateBehavior();
            _playerWinBehaviors  = new PlayerWinStateBehavior();

        }

        private void OnEnable()
        {
            _stateMachine.StateEntered += HandleStateEntered;
            _stateMachine.StateExited  += HandleStateExited;
        }

        private void OnDisable()
        {
            _stateMachine.StateEntered -= HandleStateEntered;
            _stateMachine.StateExited  -= HandleStateExited;
        }

        private void Start()
        {
            //
        }
        private void HandleStateEntered(string enteredState)
        {
            switch (enteredState)
            {
                case EncounterStates.SETUP:
                    _setupBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.DRAWUP:
                    _drawupBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.SELECT:
                    _selectBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.ROLLING:
                    _rollingBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.RESOLUTION:
                    _resolutionBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.AFTERMATH:
                    _aftermathBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.PLAYER_DEAD:
                    _playerDeadBehaviors.DoStateEnteredBehavior(this);
                    return;
                case EncounterStates.PLAYER_WIN:
                    _playerWinBehaviors.DoStateEnteredBehavior(this);
                    return;
                default:
                    return;
            }
        }
        private void HandleStateExited(string exitedState)
        {
            switch (exitedState)
            {
                case EncounterStates.SETUP:
                    _setupBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.DRAWUP:
                    _drawupBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.SELECT:
                    _selectBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.ROLLING:
                    _rollingBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.RESOLUTION:
                    _resolutionBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.AFTERMATH:
                    _aftermathBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.PLAYER_DEAD:
                    _playerDeadBehaviors.DoStateExitedBehavior(this);
                    return;
                case EncounterStates.PLAYER_WIN:
                    _playerWinBehaviors.DoStateExitedBehavior(this);
                    return;
                default:
                    return;
            }
        }
    }
}

