using UnityEngine;
using DiceTools;
using TMPro;

public class SpinnerTester : MonoBehaviour
{
    [SerializeField] private TMP_Text _uIText;

    private Spinner _spinner;

    private int myCounter = 0;

    private void Awake()
    {
        _spinner = new Spinner();
    }

    private void OnEnable()
    {
        _spinner.SpinningStarted  += HandleSpinnerStarted;
        _spinner.SpinningFinished += HandleSpinnerFInished;
        _spinner.SpinnerUpdated   += HandleSpinnerUpdated;
    }
    private void OnDisable()
    {
        _spinner.SpinningStarted  -= HandleSpinnerStarted;
        _spinner.SpinningFinished -= HandleSpinnerFInished;
        _spinner.SpinnerUpdated   -= HandleSpinnerUpdated;
    }

    private void Start()
    {
        _spinner.Spin(gameObject, 5f, 50);
    }

    private void HandleSpinnerStarted()
    {
        Debug.Log($"### {name}: Spinner started.");
    }
    private void HandleSpinnerFInished()
    {
        Debug.Log($"### {name}: Spinner finished.");
    }

    private void HandleSpinnerUpdated()
    {
        myCounter++;
        _uIText.text = myCounter.ToString();
        Debug.Log($"### {name}: 'CLICK...'");
    }

}
