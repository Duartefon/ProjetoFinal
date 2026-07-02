using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Gameplay
{
    public class GeneratorScript : MonoBehaviour
    {
        [Header("Estado do Gerador")]
        private bool _isOn = false;
        private bool _energyEstablished = false;
        //getters publicos
        public bool IsOn => _isOn;
        public bool EnergyEstablished => _energyEstablished;

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
        public static event Action<bool> OnPowerChanged;

        void Start()
        {
            if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey))
            {
                _isComplete = true;
                _isOn = true;
                _energyEstablished = true;
                PlayAmbientSound();
                OnPowerChanged?.Invoke(true);
                OnEnergyEstablished?.Invoke();
            }
        }

        void Update()
        {
            if (_energyEstablished && !audioSource.isPlaying)
                PlayAmbientSound();

            if (!_energyEstablished && _isOn && leverSocket.hasSelection && IsAtTargetVoltage())
                EstablishEnergy();
        }

        private void EstablishEnergy()
        {
            _energyEstablished = true;
            OnEnergyEstablished?.Invoke();

            if (!_isComplete)
            {
                _isComplete = true;
                Debug.Log("Generator: puzzle completed!");
                PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            }
        }

        // chamado pelo VoltageScript via onVoltageChanged
        public void SetVoltage(float newVoltage)
        {
            _currentVoltage = newVoltage;
        }

        public void AddFuse()
        {
            _isOn = true;
            OnPowerChanged?.Invoke(true);
        }

        public void RemoveFuse()
        {
            _isOn = false;
            OnPowerChanged?.Invoke(false);

            if (_energyEstablished)
            {
                _energyEstablished = false;
                OnEnergyLost?.Invoke();
            }
        }

        private bool IsAtTargetVoltage()
            => Mathf.Abs(_currentVoltage - targetVoltage) <= voltageTolerance;

        private void CompleteGenerator()
        {
            Debug.Log("Generator: energy established!");
            _energyEstablished = true;
            _isComplete = true;
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            OnEnergyEstablished?.Invoke();
        }

        private void PlayAmbientSound()
        {
            if (audioSource && !audioSource.isPlaying)
                audioSource.Play();
        }
    }
}