using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioClip openSound, closeSound;
    private AudioSource audioSource;
    private Animator animator;
    private bool isOpen = false;
    private GeneratorScript generatorScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generatorScript = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (generatorScript.isOn && !isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }

    public void PlayOpenSound() { audioSource.PlayOneShot(openSound); }

    public void PlayCloseSound() { audioSource.PlayOneShot(closeSound); }

    public void CloseDoor()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
            isOpen = false;
        }
    }
}
