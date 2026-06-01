using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private DoorController door;

    private void Awake()
    {
        // Automatically grabs the DoorController from the parent
        door = GetComponentInParent<DoorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            door.OpenDoor();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            door.CloseDoor();
    }
}
