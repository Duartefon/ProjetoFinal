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
    private static float failProbability = 0.2f; // Probabilidade de falha de cada luz

    [Header("Materials")]
    public Material onMaterial; // Material para quando a luz está ligada
    public Material offMaterial; // Material para quando a luz está desligada

    [Header("Light Mesh")]
    public MeshRenderer lightMesh; // Mesh da luz para mudar o material

    void Start()
    {
        generatorScript = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        lightComponents = GetComponentsInChildren<Light>();
    }


    void Update()
    {
        if(generatorScript.isOn && !isOn) // Se o gerador estiver ligado e a luz estiver desligada
        {
            float delay = Random.Range(0.5f, 2f); // Gerar um delay aleatório entre 0.5 e 2 segundos
            Invoke("TurnOnLight", delay); // Chamar a função para ligar a luz após o delay
        }
        else if(!generatorScript.isOn && isOn) {
            TurnOffLight();
        }

        if(isOn)
        {
            lightMesh.material = onMaterial;
        }
        else
        {
            lightMesh.material = offMaterial;
        }
    }

    private void TurnOnLight()
    {
        if(Random.value > failProbability)
        {
            foreach(Light light in lightComponents)
            {
                light.enabled = true;
            }
            isOn = true;
        }
        else
        {
            Debug.Log("A luz falhou ao ligar!");
        }
    }

    private void TurnOffLight()
    {
        foreach(Light light in lightComponents)
        {
            light.enabled = false;
        }
        isOn = false;
    }
}
