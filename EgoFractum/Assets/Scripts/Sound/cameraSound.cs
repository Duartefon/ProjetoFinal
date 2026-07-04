using UnityEngine;

public class cameraSOund : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioSource _audioSource;
    
    void Start()
    {
        
    }

    public void OnPlay()
    {
        _audioSource.Play();
    }
}
