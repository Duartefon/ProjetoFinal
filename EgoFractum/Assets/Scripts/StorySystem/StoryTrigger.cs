using UnityEngine;

namespace StorySystem
{
    public class StoryTrigger : MonoBehaviour
    {
        private StoryManager _storyManager;
        public AudioClip voiceLine;

        private void Awake()
        {
            _storyManager = GetComponentInParent<StoryManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _storyManager.PlayVoiceLine(voiceLine);
        }

        private void OnTriggerExit(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}
