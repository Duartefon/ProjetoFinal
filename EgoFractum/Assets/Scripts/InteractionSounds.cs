using UnityEngine;

public class InteractionSounds : MonoBehaviour
{
    public enum SoundType
    {
        Hit, // quando um item bate nalgo
        Touch, // quando um item é tocado
    }

    public SoundType soundType;
    public float hitThreshold = 3f; // velocidade mínima para considerar um impacto
    public AudioClip soundEffect;
    private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (soundType == SoundType.Hit)
        {
            float impactVelocity = collision.relativeVelocity.magnitude;

            if (impactVelocity > hitThreshold)
            {
                audioSource.PlayOneShot(soundEffect);
            }

        } else if (soundType == SoundType.Touch && collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
