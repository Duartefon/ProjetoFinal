using System;
using UnityEngine;

public class ScalePlate : MonoBehaviour
{
    private float _currentMass = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Weight")) return;
       
        var mass = other.GetComponent<Rigidbody>().mass;
       
        _currentMass += mass;
    }
    
    private void OnTriggerExit(Collider other)
    { 
        if (!other.gameObject.CompareTag("Weight")) return;
       
        _currentMass -= other.GetComponent<Rigidbody>().mass;
    }

    public float GetCurrentMass()
    {
        return _currentMass;
    }
}
