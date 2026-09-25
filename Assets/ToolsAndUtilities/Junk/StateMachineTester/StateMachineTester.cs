using UnityEngine;
using SimpleStateMachine;
using UnityEngine.UI;
using EventChannels;

public class StateMachineTester : MonoBehaviour
{
    private Image _image;
    [SerializeField] private SO_SimpleStateMachine _stateMachineData;
    [SerializeField] private SO_EventStringPayload _colorButtonPressedEvent;
    private RuntimeSimpleStateMachine _stateMachine = null;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _stateMachine = _stateMachineData.GetRuntimeSimpleStateMachine();
        
    }

    private void OnEnable()
    {
        _stateMachine.StateEntered += HandleStateEntered;
        _colorButtonPressedEvent.OnEventTriggered += HandleColorButtonPressed;
    }
    private void OnDisable()
    {
        _stateMachine.StateEntered -= HandleStateEntered;
        _colorButtonPressedEvent.OnEventTriggered -= HandleColorButtonPressed;
    }

    private void HandleStateEntered(string enteredState)
    {
        switch (enteredState)
        {
            case "GREEN":
                _image.color = Color.green;
                return;
            case "YELLOW":
                _image.color = Color.yellow;
                return;
            case "RED":
                _image.color = Color.red;
                return;

            default:
                Debug.Log("FOO");
                return;
        }
    }

    private void Start()
    {
        _image.color = Color.green;
    }

    private void HandleColorButtonPressed(string buttonString)
    {
        _stateMachine.TryTakeTransition(buttonString);
    }


}
