using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class GeneratorScript : MonoBehaviour
{
    public bool isOn = false;
    public bool energyEstablished = false;

    [SerializeField] private float targetVoltage = 600f;
    [SerializeField] private float voltageTolerance = 0.1f; // para comparar floats

    public AudioSource audioSource;
    public XRSocketInteractor leverSocket;

    public string puzzleKey = "Generator";

    private float _currentVoltage = 0f;
    private bool _isComplete = false;

    [Header("Events")]
    public UnityEvent onEnergyEstablished;
    public UnityEvent onEnergyLost;
    public bool Established => energyEstablished;

    void Start()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey))
        {
            _isComplete = true;
            isOn = true;
            energyEstablished = true;
            PlayAmbientSound();
            onEnergyEstablished?.Invoke();
        }
    }

    void Update()
    {
        if (energyEstablished && !audioSource.isPlaying)
            PlayAmbientSound();

        if (!_isComplete && isOn && leverSocket.hasSelection && IsAtTargetVoltage())
            CompleteGenerator();


        // Teste
        if(energyEstablished) CompleteGenerator();
    }

    // chamado pelo VoltageScript via onVoltageChanged
    public void SetVoltage(float newVoltage)
    {
        _currentVoltage = newVoltage;
    }

    public void AddFuse()
    {
        isOn = true;
    }

    public void RemoveFuse()
    {
        isOn = false;
        energyEstablished = false;

        onEnergyLost?.Invoke();
    }

    private bool IsAtTargetVoltage()
        => Mathf.Abs(_currentVoltage - targetVoltage) <= voltageTolerance;

    private void CompleteGenerator()
    {
        Debug.Log("Generator: energy established!");
        energyEstablished = true;
        _isComplete = true;
        PuzzleManager.Instance.CompletePuzzle(puzzleKey);
        onEnergyEstablished?.Invoke();
    }

    private void PlayAmbientSound()
    {
        if (audioSource && !audioSource.isPlaying)
            audioSource.Play();
    }
}