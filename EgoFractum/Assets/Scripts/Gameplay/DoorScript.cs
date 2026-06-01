using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private GeneratorScript generatorScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generatorScript = GameObject.FindWithTag("Generator").GetComponent<GeneratorScript>();
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (generatorScript.isOn && !isOpen)
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
}
