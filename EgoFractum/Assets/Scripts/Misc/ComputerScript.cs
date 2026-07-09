using Gameplay;
using UnityEngine;

public class ComputerScript : MonoBehaviour
{
    [SerializeField] private Light screenLight;
    [SerializeField] private Material screenOnMaterial, screenOffMaterial;
    [SerializeField] private MeshRenderer screenMesh;

    private void OnEnable()
    { 
        GeneratorScript.OnEnergyEstablished += TurnOn;
        GeneratorScript.OnEnergyLost += TurnOff;
    }
    
    private void OnDisable()
    {
        GeneratorScript.OnEnergyEstablished -= TurnOn;
        GeneratorScript.OnEnergyLost -= TurnOff;
    }

    private void TurnOn()
    {
        screenMesh.material = screenOnMaterial;
        screenLight.enabled = true;
    }

    private void TurnOff()
    {
        screenMesh.material = screenOffMaterial;
        screenLight.enabled = false;
    }
}
