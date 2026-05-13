using TMPro;
using UnityEngine;
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

    [Range(0, 1000)]
    public float voltage = 0f;
    private float previousVoltage;

    void Start()
    {
        voltageText.color = lowVoltageColor;
        voltageText.text = $"{voltage}V";
        screenLight.color = lowVoltageColor;

        previousVoltage = voltage;
    }
    void Update()
    {
        if (generator.isOn)
        {
            voltageText.enabled = true;
            screenLight.enabled = true;

            generator.SetVoltage(voltage);

            if (voltage != previousVoltage)
            {
                if (voltage == goodVoltage)
                    BeepSuccess();
                else
                    Beep();

                previousVoltage = voltage;
            }

            Color targetColor;
            if (voltage <= goodVoltage)
            {
                targetColor = Color.Lerp(lowVoltageColor, goodVoltageColor,
                    Mathf.InverseLerp(0, goodVoltage, voltage));
            }
            else
            {
                targetColor = Color.Lerp(goodVoltageColor, mediumVoltageColor,
                    Mathf.InverseLerp(goodVoltage, maxVoltage, voltage));
            }

            voltageText.color = targetColor;
            screenLight.color = targetColor;
            voltageText.text = $"{voltage}V";

            if (voltage < lowVoltage)
            {
                Pulse();
            } else {
                screenLight.intensity = maxLightIntensity;
            }
        }
         else
        {
            voltageText.enabled = false;
            screenLight.enabled = false;
            voltageText.text = "0V";
        }
    }
    void Pulse()
    {
        voltageText.alpha = 0.5f + 0.5f * Mathf.Sin(Time.time * pulseFrequency);
        screenLight.intensity = Mathf.Clamp(
            0.5f + 0.5f * Mathf.Sin(Time.time * pulseFrequency),
            0, maxLightIntensity);
    }

    void Beep()
    {
        if (audioSource && beepSound)
        {
            audioSource.PlayOneShot(beepSound);
        }
    }

    void BeepSuccess()
    {
        if (audioSource && beepSuccessSound)
        {
            audioSource.PlayOneShot(beepSuccessSound);
        }
    }
}