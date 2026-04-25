using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The light component to flicker. If empty, it grabs the one on this object.")]
    [SerializeField] private Light targetLight;

    [Tooltip("Minimum time the light stays in one state (seconds).")]
    [SerializeField] private float minDelay = 0.05f;

    [Tooltip("Maximum time the light stays in one state (seconds).")]
    [SerializeField] private float maxDelay = 0.2f;

    [Tooltip("Chance to stay ON during a flicker cycle (0.0 to 1.0). Higher = mostly on.")]
    [Range(0f, 1f)]
    [SerializeField] private float stability = 0.7f;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sparkSound;

    private void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (targetLight != null)
            StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // 1. Randomly decide if light is ON or OFF based on stability
            bool isLightOn = Random.value < stability;
            targetLight.enabled = isLightOn;

            // 2. Optional: Play sound only when it sparks OFF
            if (!isLightOn && audioSource != null && sparkSound != null)
            {
                // Play randomly to avoid repetitive machine-gun sound
                if (Random.value > 0.5f) 
                    audioSource.PlayOneShot(sparkSound);
            }

            // 3. Wait for a random short duration
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}