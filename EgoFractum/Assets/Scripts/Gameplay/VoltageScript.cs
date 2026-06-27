using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class VoltageScript : MonoBehaviour
{
    [Header("Colors")]
    public Color lowVoltageColor;
    public Color mediumVoltageColor;
    public Color goodVoltageColor;

    [Header("References")]
    public TMP_Text voltageText;
    public Light screenLight;
    public GeneratorScript generator;

    [Header("Light Settings")]
    public float maxLightIntensity = 0.25f;

    [Header("Voltage Settings")]
    public float maxVoltage = 1000f;
    public float goodVoltage = 600f;
    public float lowVoltage = 100f;
    public float pulseFrequency = 6f;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip beepSound;
    public AudioClip beepSuccessSound;

    [Header("Events")]
    public UnityEvent<float> onVoltageChanged;

    [Range(0, 1000)]
    private float _voltage = 0f;
    public float Voltage => _voltage; // getter

    private float _previousVoltage;

    void Start()
    {
        _previousVoltage = _voltage;
        UpdateVisuals();

        if (generator.energyEstablished)
            SetVoltage(goodVoltage);
    }

    void Update()
    {
        bool generatorOn = generator.isOn;

        voltageText.enabled = generatorOn;
        screenLight.enabled = generatorOn;

        if (!generatorOn)
        {
            voltageText.text = "0V";
            return;
        }

        if (generator.energyEstablished && _voltage != goodVoltage)
            SetVoltage(goodVoltage);

        if (_voltage != _previousVoltage)
        {
            PlayFeedbackSound();
            generator.SetVoltage(_voltage);
            onVoltageChanged?.Invoke(_voltage);
            _previousVoltage = _voltage;
        }

        UpdateVisuals();

        if (_voltage < lowVoltage)
            Pulse();
        else
            screenLight.intensity = maxLightIntensity;
    }

    public void AdjustVoltage(float delta)
    {
        SetVoltage(Mathf.Clamp(_voltage + delta, 0f, maxVoltage));
    }

    public void SetVoltage(float newVoltage)
    {
        if (!generator.isOn) return;
        _voltage = Mathf.Clamp(newVoltage, 0f, maxVoltage);
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