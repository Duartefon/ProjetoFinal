using System;
using Gameplay;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GeneratorScript generator;
    public AudioClip openSound, closeSound;
    private AudioSource audioSource;
    private Animator animator;
    private bool isOpen = false;
    private bool energyEstablished = false;

    private void OnEnable()
    {
        GeneratorScript.OnEnergyEstablished += OnEnergyEstablished;
        GeneratorScript.OnEnergyLost += OnEnergyLost;
    }
    
    private void OnDisable()
    {
        GeneratorScript.OnEnergyEstablished -= OnEnergyEstablished;
        GeneratorScript.OnEnergyLost -= OnEnergyLost;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        generator = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        //generator.onEnergyEstablished.AddListener(OnEnergyEstablished);
      //  generator.onEnergyLost.AddListener(OnEnergyLost);

        if (generator.energyEstablished)
            energyEstablished = true;
    }

    void OnDestroy()
    {
       // generator.onEnergyEstablished.RemoveListener(OnEnergyEstablished);
     //   generator.onEnergyLost.RemoveListener(OnEnergyLost);
    }

    public void OnEnergyEstablished() => energyEstablished = true;

    public void OnEnergyLost() => energyEstablished = false;

    public void OpenDoor()
    {
        if (energyEstablished && !isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
            isOpen = false;
        }
    }

    public void PlayOpenSound() { audioSource.PlayOneShot(openSound); }

    public void PlayCloseSound() { audioSource.PlayOneShot(closeSound); }
}
