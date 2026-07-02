using Gameplay;
using UnityEngine;

public class ComputerScript : MonoBehaviour
{
    [SerializeField] private Light screenLight;
    [SerializeField] private Material screenOnMaterial, screenOffMaterial;
    private GeneratorScript generatorScript;
    [SerializeField] private MeshRenderer screenMesh;
    void Start()
    {
        generatorScript = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
    }

    void Update()
    {
        if (generatorScript.EnergyEstablished)
        {
            screenMesh.material = screenOnMaterial;
            screenLight.enabled = true;
        }
        else
        {
            screenMesh.material = screenOffMaterial;
            screenLight.enabled = false;
        }
    }
}
