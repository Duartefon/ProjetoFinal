using UnityEngine;

public class ComputerScript : MonoBehaviour
{
    public Light screenLight;
    public Material screenOnMaterial, screenOffMaterial;
    private GeneratorScript generatorScript;
    public MeshRenderer screenMesh;
    void Start()
    {
        generatorScript = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
    }

    void Update()
    {
        if (generatorScript.energyEstablished)
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
