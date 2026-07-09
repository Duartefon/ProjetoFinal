using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractionSounds : MonoBehaviour
{
    private enum SoundType
    {
        Hit, // quando um item bate em algo
        Touch, // quando um item é tocado
    }

    [SerializeField] private SoundType soundType;
    [SerializeField] private float hitThreshold = 3f; // velocidade mínima para considerar um impacto
    [SerializeField] private AudioClip soundEffect;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (soundType == SoundType.Hit)
        {
            float impactVelocity = collision.relativeVelocity.magnitude;

            if (impactVelocity > hitThreshold)
            {
                _audioSource.PlayOneShot(soundEffect);
            }

        } else if (soundType == SoundType.Touch && collision.gameObject.CompareTag("Player"))
        {
            _audioSource.PlayOneShot(soundEffect);
        }
    }
}
