using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class VoltageScript : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color lowVoltageColor;
        [SerializeField] private Color mediumVoltageColor;
        [SerializeField] private Color goodVoltageColor;

        [Header("References")]
        [SerializeField] private TMP_Text voltageText;
        [SerializeField] private Light screenLight;
        [SerializeField] private GeneratorScript generator;

        [Header("Light Settings")]
        [SerializeField] private float maxLightIntensity = 0.25f;

        [Header("Voltage Settings")]
        [SerializeField] private float maxVoltage = 1000f;
        [SerializeField] private float goodVoltage = 600f;
        [SerializeField] private float lowVoltage = 100f;
        [SerializeField] private float pulseFrequency = 6f;

        [Header("Sound Effects")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip beepSound;
        [SerializeField] private AudioClip beepSuccessSound;

        [Header("Events")]
        public static Action<float> OnVoltageChanged;

        [Range(0, 1000)]
        private float _voltage = 0f;
        public float Voltage => _voltage; // getter

        private float _previousVoltage;
        private bool _generatorOn;
        
        private void OnEnable()
        {
            KnobRotator.OnValueChanged += AdjustVoltage;
            GeneratorScript.OnPowerChanged += HandlePower;
            GeneratorScript.OnEnergyEstablished += EnergyOn;
        }

        private void OnDisable()
        {
            KnobRotator.OnValueChanged -= AdjustVoltage;
            GeneratorScript.OnPowerChanged -= HandlePower;
            GeneratorScript.OnEnergyEstablished -= EnergyOn;
        }

        void Start()
        {
            _generatorOn = generator.IsOn;
            voltageText.enabled = _generatorOn;
            screenLight.enabled = _generatorOn;

            if (generator.EnergyEstablished)
                SetVoltage(goodVoltage);
            else
                UpdateVisuals();
        }

        void Update()
        {
            if (generator.EnergyEstablished && _voltage != goodVoltage)
                SetVoltage(goodVoltage);

            if (_voltage != _previousVoltage)
            {
                PlayFeedbackSound();
                generator.SetVoltage(_voltage);
                OnVoltageChanged?.Invoke(_voltage);
                _previousVoltage = _voltage;
            }

            if (!_generatorOn) return;

            if (_voltage < lowVoltage)
                Pulse();
            else
                screenLight.intensity = maxLightIntensity;
        }
        
        private void HandlePower(bool isOn)
        {
            _generatorOn = isOn;
            voltageText.enabled = isOn;
            screenLight.enabled = isOn;
            
            if(!isOn) voltageText.text = "0V";
        }

        private void EnergyOn()
        {
            
        }

        public void AdjustVoltage(float delta)
        {
            SetVoltage(Mathf.Clamp(_voltage + delta, 0f, maxVoltage));
        }

        public void SetVoltage(float newVoltage)
        {
            if (!generator.IsOn) return;
            _voltage = Mathf.Clamp(newVoltage, 0f, maxVoltage);
            
            generator.SetVoltage(_voltage);
            OnVoltageChanged?.Invoke(_voltage);
            UpdateVisuals();
            PlayFeedbackSound();
        }

        private void UpdateVisuals()
        {
            Color targetColor;

            if (_voltage <= goodVoltage)
                targetColor = Color.Lerp(lowVoltageColor, goodVoltageColor,
                    Mathf.InverseLerp(0, goodVoltage, _voltage));
            else
                targetColor = Color.Lerp(goodVoltageColor, mediumVoltageColor,
                    Mathf.InverseLerp(goodVoltage, maxVoltage, _voltage));

            voltageText.color = targetColor;
            voltageText.text = $"{_voltage}V";
            screenLight.color = targetColor;
        }

        private void PlayFeedbackSound()
        {
            if (!audioSource) return;

            if (_voltage == goodVoltage && beepSuccessSound)
                audioSource.PlayOneShot(beepSuccessSound);
            else if (beepSound)
                audioSource.PlayOneShot(beepSound);
        }

        void Pulse()
        {
            float sin = 0.5f + 0.5f * Mathf.Sin(Time.time * pulseFrequency);
            voltageText.alpha = sin;
            screenLight.intensity = Mathf.Clamp(sin, 0, maxLightIntensity);
        }
    }
}