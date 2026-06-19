using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Light targetLight;
    [SerializeField] private float minDelay = 0.05f;
    [SerializeField] private float maxDelay = 0.2f;
    [Range(0f, 1f)]
    [SerializeField] private float stability = 0.7f;
    [SerializeField] private float smoothSpeed = 18f;// quao rapido a intensidade se ajusta ao valor alvo
                                                     // maior = mais rapido, menor = mais suave

    [Header("Burst Settings")]
    [SerializeField] private float minRestTime = 1.5f;
    [SerializeField] private float maxRestTime = 5f;
    [SerializeField] private int minFlickerBurst = 2;
    [SerializeField] private int maxFlickerBurst = 6;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip loopSound;

    [Header("Materials (Optional)")]
    [SerializeField] public MeshRenderer lightMesh;
    [SerializeField] public Material onMaterial;
    [SerializeField] public Material offMaterial;

    private float targetIntensity;
    private float fullIntensity;
    private Coroutine flickerCoroutine;

    private void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (targetLight == null)
            return;

        fullIntensity = targetLight.intensity;
        targetIntensity = fullIntensity;

        if (audioSource != null && loopSound != null)
        {
            audioSource.clip = loopSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    private void Update()
    {
        if (targetLight == null)
            return;

        targetLight.intensity = Mathf.MoveTowards(
            targetLight.intensity,
            targetIntensity,
            smoothSpeed * Time.deltaTime);
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            int bursts = Random.Range(minFlickerBurst, maxFlickerBurst + 1);

            for (int i = 0; i < bursts; i++)
            {
                bool isOn = Random.value < stability;

                targetLight.enabled = isOn;
                targetIntensity = isOn ? fullIntensity : 0f;

                if (lightMesh != null && onMaterial != null && offMaterial != null)
                    lightMesh.material = isOn ? onMaterial : offMaterial;

                if (audioSource != null)
                {
                    if (isOn)
                    {
                        if (!audioSource.isPlaying)
                            audioSource.Play();
                    }
                    else
                    {
                        if (audioSource.isPlaying)
                            audioSource.Stop();
                    }
                }

                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }

            targetLight.enabled = true;
            targetIntensity = fullIntensity;

            if (lightMesh != null && onMaterial != null)
                lightMesh.material = onMaterial;

            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();

            yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
        }
    }

    private void OnDisable()
    {
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}