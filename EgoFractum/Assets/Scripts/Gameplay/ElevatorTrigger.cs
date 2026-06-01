using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private ElevatorController elevatorController;
    void Start()
    {
        elevatorController = GetComponentInParent<ElevatorController>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            other.transform.SetParent(transform.parent);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if(rb != null) {
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            other.transform.SetParent(null);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if(rb != null) {
                rb.isKinematic = false;
            }
        }
    }
}
