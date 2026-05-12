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

    [Header("Light Settings")]
    public float maxLightIntensity = 0.25f;

    [Header("Voltage Settings")]
    public float maxVoltage = 1000f;
    public float goodVoltage = 600f;
    public float lowVoltage = 100f;
    public float pulseFrequency = 6f;

    [Range(0, 1000)]
    public float voltage = 0f;
    
    void Start()
    {
        voltageText.color = lowVoltageColor;
        voltageText.text = $"{voltage}V";
        screenLight.color = lowVoltageColor;
    }
    void Update()
    {
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
        }
    }
    void Pulse()
    {
        voltageText.alpha = 0.5f + 0.5f * Mathf.Sin(Time.time * pulseFrequency);
        screenLight.intensity = Mathf.Clamp(
            0.5f + 0.5f * Mathf.Sin(Time.time * pulseFrequency),
            0, maxLightIntensity);
    }
}