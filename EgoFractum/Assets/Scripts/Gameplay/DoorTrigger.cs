using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private DoorController _door;

    private void Awake()
    {
        _door = GetComponentInParent<DoorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _door.OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _door.CloseDoor();
    }
}
