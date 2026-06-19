using UnityEngine;

/**
* Este script vai controlar as luzes do mapa consoante a energia do gerador.
* Quando ligadas, cada luz vai ter um delay diferente, para criar um efeito de luzes a acenderem-se uma a uma.
* Para além disso, existe uma probabilidade de cada luz falhar, ou seja, de não acender quando o gerador é ligado.
* O script vai ser colocado em cada luz do mapa.
*/
public class LightScript : MonoBehaviour
{
    private GeneratorScript generatorScript; // Referência ao script do gerador
    private Light[] lightComponents; // Cada luz tem dois componentes light
    private bool isOn = false; // Estado da luz
    private bool isPendingOn = false; // Se a luz está à espera de acender
    private static float failProbability = 0.2f; // Probabilidade de falha de cada luz

    [Header("Materials")]
    public Material onMaterial; // Material para quando a luz está ligada
    public Material offMaterial; // Material para quando a luz está desligada

    [Header("Light Mesh")]
    public MeshRenderer lightMesh; // Mesh da luz para mudar o material

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip lightBuzz, lightOnSound, lightOffSound;

    void Start()
    {
        generatorScript = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        lightComponents = GetComponentsInChildren<Light>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (generatorScript.energyEstablished && !isOn && !isPendingOn) // Se o gerador estiver ligado e a luz estiver desligada
        {
            isPendingOn = true;
            float delay = Random.Range(0.5f, 2f); // Gerar um delay aleatório entre 0.5 e 2 segundos
            Invoke("TurnOnLight", delay); // Chamar a função para ligar a luz após o delay
        }
        else if (!generatorScript.energyEstablished && isOn)
        {
            TurnOffLight();
        }

        if (isOn)
        {
            if (audioSource != null)
            {
                audioSource.clip = lightBuzz;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
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
                lightMesh.material = onMaterial;
            }

            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.75f, 0.85f);
                audioSource.PlayOneShot(lightOnSound);
            }
        }
        else
        {
            // se a luz falhar vai tocar um som de faisca e vai se adicionar
            // o script LightFlicker para criar um efeito de luz a falhar
            /*
            foreach (Light light in lightComponents) {
                light.gameObject.AddComponent<LightFlicker>();
            }
            */

            foreach (Light light in lightComponents)
            {
                light.enabled = false;
                lightMesh.material = offMaterial;
            }

        }

        isOn = true;
    }

    private void TurnOffLight()
    {
        CancelInvoke("TurnOnLight"); // Cancelar qualquer Invoke pendente
        isPendingOn = false;
        isOn = false;

        foreach (Light light in lightComponents)
            light.enabled = false;

        if (audioSource != null)
            audioSource.PlayOneShot(lightOffSound);
    }
}