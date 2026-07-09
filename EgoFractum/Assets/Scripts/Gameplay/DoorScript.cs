using System;
using Gameplay;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GeneratorScript generator;
    [SerializeField] private AudioClip openSound, closeSound;
    private AudioSource _audioSource;
    private Animator _animator;
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
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (generator == null)
        {
            var generatorObj = GameObject.FindWithTag("Generator");
            if (generatorObj != null)
                generator = generatorObj.GetComponent<GeneratorScript>();
        }

        if (generator.EnergyEstablished)
            _energyEstablished = true;
    }

    private void OnEnergyEstablished() => _energyEstablished = true;

    private void OnEnergyLost() => _energyEstablished = false;

    public void OpenDoor()
    {
        if (_energyEstablished && !_isOpen)
        {
            _animator.SetTrigger("Open");
            _isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (_isOpen)
        {
            _animator.SetTrigger("Close");
            _isOpen = false;
        }
    }

    public void PlayOpenSound()
    {
        _audioSource.PlayOneShot(openSound);
    }

    public void PlayCloseSound()
    {
        _audioSource.PlayOneShot(closeSound);
    }
}