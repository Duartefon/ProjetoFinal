using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Gameplay
{
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

       [HideInInspector]
        public static event Action OnEnergyEstablished;
        public static event Action OnEnergyLost;


        [Header("Dependencies")]
        // private ElevatorController _elevatorController;
        public bool Established => energyEstablished;


        private void OnEnable()
        {
            //  onEnergyEstablished += _elevatorController.OnEnergyEstablished; // += onEnergyEstablished;
        }

        void Start()
        {
            if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey))
            {
                _isComplete = true;
                isOn = true;
                energyEstablished = true;
                PlayAmbientSound();
                //     OnEnergyEstablished?.Invoke();
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

            //  OnEnergyLost?.Invoke();
        }

        private bool IsAtTargetVoltage()
            => Mathf.Abs(_currentVoltage - targetVoltage) <= voltageTolerance;

        private void CompleteGenerator()
        {
            Debug.Log("Generator: energy established!");
            energyEstablished = true;
            _isComplete = true;
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            //     OnEnergyEstablished?.Invoke();
        }

        private void PlayAmbientSound()
        {
            if (audioSource && !audioSource.isPlaying)
                audioSource.Play();
        }
    }
}