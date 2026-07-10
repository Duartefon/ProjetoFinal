using System;
using UnityEngine;

public class ScalePlate : MonoBehaviour
{
    private float _currentMass = 0;
    private void OnTriggerEnter(Collider other)
    {
        var mass = other.GetComponent<Rigidbody>().mass;
       Debug.Log("Entrou: " + other.attachedRigidbody.name + ", com peso: " + mass);
       _currentMass += mass;
    }
    
    private void OnTriggerExit(Collider other)
    { 
        Debug.Log("Saiu: " + other.attachedRigidbody.name);
        _currentMass -= other.GetComponent<Rigidbody>().mass;
    }

    public float GetCurrentMass()
    {
        return _currentMass;
    }
}
