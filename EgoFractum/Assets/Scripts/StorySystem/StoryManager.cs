using UnityEngine;

namespace StorySystem
{
    public class StoryManager : MonoBehaviour
    {
        public AudioSource audioSource;

        public void PlayVoiceLine(AudioClip voiceLine) => audioSource.PlayOneShot(voiceLine);
    }
}
