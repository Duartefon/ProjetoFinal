using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GeneratorScript : MonoBehaviour
{
    private int fuseCount = 2;
    public bool isOn = false;
    private float voltage = 0;
    private AudioSource audioSource;
    public XRSocketInteractor leverSocket, fuseSocket;


    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fuseSocket.hasSelection)
        {
            AddFuse();
        }

        if(isOn)
        {
            audioSource.Play();
        }

        if(voltage == 600f && leverSocket.hasSelection)
        {
            Debug.Log("Everything is on!");
        }
    }

    public void AddFuse()
    {
        isOn = true;
    }

    public void SetVoltage(float voltage)
    {
        // 600V
        this.voltage = voltage;
    }


}
