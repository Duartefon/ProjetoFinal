using System;
using UnityEngine;

public class ScalePlate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       Debug.Log("Entrou: " + other.attachedRigidbody.name);
       
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Saiu: " + other.attachedRigidbody.name);
       
    }
}
