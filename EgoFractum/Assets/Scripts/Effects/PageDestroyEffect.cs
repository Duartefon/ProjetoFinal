using UnityEngine;

public class PageDestroyEffect : MonoBehaviour
{
    [Header("References")]
    [Tooltip("O Rigidbody do objeto dinâmico sob as páginas")]
    public Rigidbody objectRigidbody;
    public ParticleSystem pageParticleSystem;
    public AudioSource audioSource;

    [Header("Settings")]
    [Tooltip("O quão rápido o Rigidbody tem de se mexer para as páginas serem destruídas")]
    public float movementThreshold = 0.5f;


    void Update()
    {
        if (objectRigidbody != null && objectRigidbody.linearVelocity.magnitude > movementThreshold)
        {
            DestroyPages();
        }
    }

    private void DestroyPages()
    {
        if (pageParticleSystem != null)
        {
            ParticleSystem ps = Instantiate(
                pageParticleSystem,
                transform.position,
                pageParticleSystem.transform.rotation
            );
            ps.Emit(4);
        }
        if (audioSource != null) AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, audioSource.volume);
        Destroy(gameObject);
    }
}
