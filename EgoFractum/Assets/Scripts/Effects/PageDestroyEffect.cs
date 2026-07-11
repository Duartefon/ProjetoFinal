using UnityEngine;

public class PageDestroyEffect : MonoBehaviour
{
    [Header("References")]
    [Tooltip("O Rigidbody do objeto dinâmico sob as páginas")]
    public Rigidbody objectRigidbody;
    public ParticleSystem pageParticleSystem;
    public AudioClip audioClip;

    [Header("Settings")]
    [Tooltip("O quão rápido o Rigidbody tem de se mexer para as páginas serem destruídas")]
    public float movementThreshold = 0.5f;

    void Update()
    {
        if (objectRigidbody && objectRigidbody.linearVelocity.magnitude > movementThreshold)
        {
            DestroyPages();
        }
    }

    private void DestroyPages()
    {
        if (pageParticleSystem)
        {
            ParticleSystem ps = Instantiate(
                pageParticleSystem,
                transform.position,
                pageParticleSystem.transform.rotation
            );
            ps.Emit(4);
        }
        if (audioClip) AudioSource.PlayClipAtPoint(audioClip, transform.position);
        Destroy(gameObject);
    }
}
