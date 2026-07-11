    using Gameplay;
using UnityEngine;

public class ScreenScript : MonoBehaviour
{
    [SerializeField] private Light screenLight;
    [SerializeField] private Material screenOnMaterial, screenOffMaterial;
    [SerializeField] private MeshRenderer screenMesh;
    private GeneratorScript _generator;

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
    
    private void Start()
    {
        _generator = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        
        if (_generator != null && _generator.EnergyEstablished)
            TurnOn();
        else
            TurnOff();
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
