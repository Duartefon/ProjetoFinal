using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    private StoryManager sm;
    public AudioClip voiceLine;

    private void Awake()
    {
        sm = GetComponentInParent<StoryManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            sm.PlayVoiceLine(voiceLine);
    }

    private void OnTriggerExit(Collider other)
    {
        //gameObject.SetActive(false);
    }
}
