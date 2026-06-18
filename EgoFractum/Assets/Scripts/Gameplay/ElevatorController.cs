using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private bool isMoving = false;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip stopSound;
    public ParticleSystem smoke;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
    }

    void Update()
    {
        if (isMoving) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                isMoving = false;
            }
        }
    }

    public void GoUp() {
        if(!isMoving) {
            isMoving = true;
            animator.SetTrigger("Up");
        }

    }

    public void GoDown() {
        if(!isMoving) {
            isMoving = true;
            animator.SetTrigger("Down");
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

    public void PlayStopSound() {
        audioSource.PlayOneShot(stopSound);
    }

    public void PlayMoveSound() {
        audioSource.PlayOneShot(moveSound);
    }
}
