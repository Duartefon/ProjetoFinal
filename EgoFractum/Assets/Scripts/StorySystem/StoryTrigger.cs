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
        
    
        private void Awake()
        {
            _storyManager = GetComponentInParent<StoryManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Is puzzle completed: " + PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey));
            if (!PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey)) return;
          
            if (other.CompareTag("Player"))
                _storyManager.PlayVoiceLine(voiceLine);
        }

        private void OnTriggerExit(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}
