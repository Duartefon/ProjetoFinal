using UnityEngine;

public class FuseScript : MonoBehaviour
{
    public GeneratorScript generator;
    private bool fuseInserted = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuse") && !fuseInserted)
        {


            // tira se a fisica para nao cair

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            //var interactable = vai buscar o interactable do fusivel
            //if (interactable != null)
                // desliga o interactable

            // quando o fusivel entra no trigger da snap
            // para  as coords focarem certas ele tem de ficar filho do gerador

            other.transform.parent = this.transform.parent;
            other.transform.position = this.transform.position;
            other.transform.rotation = this.transform.rotation;


            fuseInserted = true;
            generator.AddFuse();
        }
    }
}
