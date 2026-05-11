using UnityEngine;

public class FuseScript : MonoBehaviour
{
    public GeneratorScript generator;
    private bool fuseInserted = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuse") && !fuseInserted)
        {
            // quando o fusivel entra no trigger da snap
            other.transform.position = this.transform.position;
            other.transform.rotation = this.transform.rotation;

            // tira se a fisica para nao cair

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            fuseInserted = true;
            generator.AddFuse();
        }
    }
}
