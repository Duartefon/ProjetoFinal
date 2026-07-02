using System;
using Gameplay;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GeneratorScript generator;
    [SerializeField] private AudioClip openSound, closeSound;
    private AudioSource audioSource;
    private Animator animator;
    private bool _isOpen = false;
    private bool _energyEstablished = false;

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

        if (generator.EnergyEstablished)
            _energyEstablished = true;
    }

    void OnDestroy()
    {
       // generator.onEnergyEstablished.RemoveListener(OnEnergyEstablished);
     //   generator.onEnergyLost.RemoveListener(OnEnergyLost);
    }

    public void OnEnergyEstablished() => _energyEstablished = true;

    public void OnEnergyLost() => _energyEstablished = false;

    public void OpenDoor()
    {
        if (_energyEstablished && !_isOpen)
        {
            animator.SetTrigger("Open");
            _isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (_isOpen)
        {
            animator.SetTrigger("Close");
            _isOpen = false;
        }
    }

    public void PlayOpenSound() { audioSource.PlayOneShot(openSound); }

    public void PlayCloseSound() { audioSource.PlayOneShot(closeSound); }
}
