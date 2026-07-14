using PuzzleSystem;
using UnityEngine;

namespace StorySystem
{
    public class StoryTrigger : MonoBehaviour
    {
        private StoryManager _storyManager;
        public AudioClip voiceLine;

        [SerializeField]
        private string puzzleKey;

        public bool isVideo = false;
        private bool _hasPlayed = false;
        public AudioSource _source;
        private void Awake()
        {
            _storyManager = GetComponentInParent<StoryManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Is puzzle completed: " + PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey));
            if (!PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey)) return;

            if (other.CompareTag("Player"))
            {
                if (isVideo)
                {
                    _hasPlayed = true;
                    _source.PlayOneShot(voiceLine);
                    return;
                }
                _storyManager.PlayVoiceLine(voiceLine);
                _hasPlayed = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_hasPlayed)
                gameObject.SetActive(false);
        }
    }
}
