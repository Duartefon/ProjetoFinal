using System;
using Gameplay;
using Palmmedia.ReportGenerator.Core;
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
    
    private bool isMoving = false;
    private bool energyEstablished = false;
    private Animator animator;
    private AudioSource audioSource;
    
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
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
    }

    void Update()
    {
        if (isMoving && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            isMoving = false;
        }
    }

    private void OnEnergyEstablished() => energyEstablished = true;
    private void OnEnergyLost() => energyEstablished = false;

    public void GoUp()
    {
        if (!isMoving && energyEstablished)
        {
            isMoving = true;
            animator.SetTrigger(ElevatorUp);
            Debug.Log("Going Up");
        }
    }

    public void GoDown()
    {
        if (!isMoving && energyEstablished)
        {
            isMoving = true;
            animator.SetTrigger(ElevatorDown);
            Debug.Log("Going Down");
        }

    }
    
    public void OnArrived()
    {
        isMoving = false;
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
        audioSource.PlayOneShot(stopSound);
    }

    public void PlayMoveSound()
    {
        audioSource.PlayOneShot(moveSound);
    }
}
