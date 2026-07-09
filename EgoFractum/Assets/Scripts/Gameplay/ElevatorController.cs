using System;
using Gameplay;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private static readonly int ElevatorDown = Animator.StringToHash("ElevatorDown");
    private static readonly int ElevatorUp = Animator.StringToHash("ElevatorUp");

    [Header("Audio")] 
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip stopSound;
    
    [Header("Smoke")] 
    [SerializeField] private ParticleSystem smoke;

    private bool _isMoving = false;
    private bool _energyEstablished = false;
    private Animator _animator;
    private AudioSource _audioSource;

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
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
    }

    void Update()
    {
        if (_isMoving && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _isMoving = false;
        }
    }

    private void OnEnergyEstablished() => _energyEstablished = true;
    private void OnEnergyLost() => _energyEstablished = false;

    public void GoUp()
    {
        if (!_isMoving && _energyEstablished)
        {
            _isMoving = true;
            _animator.SetTrigger(ElevatorUp);
            Debug.Log("Going Up");
        }
    }

    public void GoDown()
    {
        if (!_isMoving && _energyEstablished)
        {
            _isMoving = true;
            _animator.SetTrigger(ElevatorDown);
            Debug.Log("Going Down");
        }
    }

    public void OnArrived()
    {
        _isMoving = false;
    }

    public void PlaySmoke()
    {
        smoke.Play();
    }

    public void StopSmoke()
    {
        smoke.Stop();
    }

    public void PlayStopSound()
    {
        _audioSource.PlayOneShot(stopSound);
    }

    public void PlayMoveSound()
    {
        _audioSource.PlayOneShot(moveSound);
    }
}