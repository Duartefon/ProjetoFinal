using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private DoorController door;

    private void Awake()
    {
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
