using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

    [RequireComponent(typeof(XRGrabInteractable))]
public class HandDummyPoseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dummyHand;
    
    [SerializeField]
    
    private GameObject playerHand;


    private void OnEnable()
    {
        var xrGrab = gameObject.GetComponent<XRGrabInteractable>();
        xrGrab.selectEntered.AddListener(OnGrab);
        xrGrab.selectExited.AddListener(OnUngrab);
        
    }
    private void OnDestroy()
    {
        var xrGrab = gameObject.GetComponent<XRGrabInteractable>();
        xrGrab.selectEntered.RemoveListener(OnGrab);
        xrGrab.selectExited.RemoveListener(OnUngrab);
        
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        dummyHand.SetActive(true);
        playerHand.SetActive(false);
        
    }
    
    
    private void OnUngrab(SelectExitEventArgs arg0)
    {
        dummyHand.SetActive(false);
        playerHand.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
