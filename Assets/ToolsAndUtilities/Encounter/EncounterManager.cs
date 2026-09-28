using UnityEngine;
using SimpleStateMachine;
using System.Collections.Generic;

namespace Encounter
{
    [RequireComponent(typeof(EncounterTestRig))]
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
        private EncounterTestRig _testRig;
        private RuntimeEncounterSetup _encounterSetup;
        private RuntimeCombatant _playerCombatant = null;
        private List<RuntimeCombatant> _enemyCombatants = new List<RuntimeCombatant>();

        private Dictionary<string, ICombatantGameObject> _combatantObjects = new Dictionary<string, ICombatantGameObject>();



        private void Awake()
        {
            _testRig = GetComponent<EncounterTestRig>();

            if ( _stateMachineSO != null)
            {
                _stateMachine = _stateMachineSO.GetRuntimeSimpleStateMachine();
            }

            _setupBehaviors      = new SetupStateBehavior();
            _drawupBehaviors     = new DrawupStateBehavior();
            _selectBehaviors     = new SelectStateBehavior();
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
            if (_stateMachine.CurrentStateName == EncounterStates.SETUP)
            {
                _setupBehaviors.DoStateEnteredBehavior(this);
            }
        }
        private void HandleStateEntered(string enteredState)
        {
            switch (enteredState)
            {
                case EncounterStates.SETUP:
                    // Setup is the initial state, so do this in Start()
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

        //================================
        // Properties and methods exposed
        // to the IStateBehaviors
        //================================
        public int MaxInHand = 5;
        public EncounterTestRig TestRig => _testRig;
        public RuntimeSimpleStateMachine StateMachine => _stateMachine;

        public RuntimeEncounterSetup EncounterSetup { get {  return _encounterSetup; } set { _encounterSetup = value; }  }

        public RuntimeCombatant PlayerCombatant { get { return _playerCombatant; } set { _playerCombatant = value; } }
        public List<RuntimeCombatant> EnemyCombatants {  get { return _enemyCombatants; } }
        public Dictionary<string, ICombatantGameObject> CombatantObjects => _combatantObjects;
        public void InstantiateCombatantObject(string id, GameObject prefab)
        {
            GameObject go = Instantiate(prefab);
            go.name = $"COMBATANT_{id}";
            ICombatantGameObject cgo = go.GetComponent<ICombatantGameObject>();
            _combatantObjects[id] = cgo;
        }
    }
}

