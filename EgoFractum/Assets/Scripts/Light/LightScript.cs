using System;
using Gameplay;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using Random = UnityEngine.Random;

/**
* Este script vai controlar as luzes do mapa consoante a energia do gerador.
* Quando ligadas, cada luz vai ter um delay diferente, para criar um efeito de luzes a acenderem-se uma a uma.
* Para além disso, existe uma probabilidade de cada luz falhar, ou seja, de não acender quando o gerador é ligado.
* O script vai ser colocado em cada luz do mapa.
*/
public class LightScript : MonoBehaviour
{
    [SerializeField] private GeneratorScript generator;
    private Light[] lightComponents; // cada luz tem dois componentes light
    private bool isOn = false; // Estado da luz
    private bool isPendingOn = false;
    private static float failProbability = 0.2f;

    [Header("Materials")]
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    [Header("Light Mesh")]
    [SerializeField] private MeshRenderer lightMesh; // Mesh da luz para mudar o material

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip lightBuzz, lightOnSound, lightOffSound;
    
    private void OnEnable()
    {
        GeneratorScript.OnEnergyEstablished += TurnOnWithDelay;
        GeneratorScript.OnEnergyLost += TurnOffLight;
    }
    private void OnDisable()
    {
        GeneratorScript.OnEnergyEstablished -= TurnOnWithDelay;
        GeneratorScript.OnEnergyLost -= TurnOffLight;
    }
    void Start()
    {
        lightComponents = GetComponentsInChildren<Light>();
        audioSource = GetComponentInChildren<AudioSource>();
/*
        generator = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        generator.OnEnergyEstablished.AddListener(TurnOnWithDelay);
        generator.onEnergyLost.AddListener(TurnOffLight);
*/
        if (generator.EnergyEstablished)
            TurnOnWithDelay();
    }

    void OnDestroy()
    {
     //   generator.onEnergyEstablished.RemoveListener(TurnOnWithDelay);
       // generator.onEnergyLost.RemoveListener(TurnOffLight);
    }

    // chamado pelo evento OnEnergyEstablished
    public void TurnOnWithDelay()
    {
        if (isOn || isPendingOn) return;
        isPendingOn = true;
        float delay = Random.Range(0.5f, 2f);
        Invoke(nameof(TurnOnLight), delay);
    }

    // chamado pelo evento OnEnergyLost
    public void TurnOffLight()
    {
        CancelInvoke(nameof(TurnOnLight));
        isPendingOn = false;
        isOn = false;

        foreach (Light light in lightComponents)
            light.enabled = false;

        lightMesh.material = offMaterial;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(lightOffSound);
            audioSource.Stop();
        }
    }

    private void TurnOnLight()
    {
        isPendingOn = false;

        if (Random.value > failProbability)
        {
            foreach (Light light in lightComponents)
            {
                light.enabled = true;
            }

            lightMesh.material = onMaterial;

            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.75f, 0.85f);
                audioSource.PlayOneShot(lightOnSound);
                audioSource.clip = lightBuzz;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // se a luz falhar vai tocar um som de faisca e vai se adicionar
            // o script LightFlicker para criar um efeito de luz a falhar
            foreach (Light light in lightComponents)
            {
                LightFlicker flicker = light.gameObject.AddComponent<LightFlicker>();
                flicker.lightMesh = lightMesh;
                flicker.onMaterial = onMaterial;
                flicker.offMaterial = offMaterial;
            }
        }

        isOn = true;
    }
}