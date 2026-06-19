using UnityEngine;

public class ElevatorController : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip moveSound;
    public AudioClip stopSound;
    [Header("Smoke")]
    public ParticleSystem smoke;
    
    private GeneratorScript generator;
    private bool isMoving = false;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
        generator = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
    }

    void Update()
    {
        if (isMoving)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                isMoving = false;
            }
        }
    }

    public void GoUp()
    {
        if (!isMoving && generator.energyEstablished)
        {
            isMoving = true;
            animator.SetTrigger("ElevatorUp");
            Debug.Log("Going Up");
        }

    }

    public void GoDown()
    {
        if (!isMoving && generator.energyEstablished)
        {
            isMoving = true;
            animator.SetTrigger("ElevatorDown");
            Debug.Log("Going Down");
        }

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
