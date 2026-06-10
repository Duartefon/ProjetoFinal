 
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    public bool isActive = false;

    public float rotationAmount = 3f;
    bool hasMusicStarted = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        
    }

    public void Play()
    {
      
        isActive = true; 
        GetComponent<AudioSource>().Play();
        
    }
    public void rotateLight()
    {   
        if (!isActive) return;
 

        transform.Rotate(0, 0, rotationAmount * Time.deltaTime, Space.World);

    }
    
    // Update is called once per frame
    void Update()
    {
        rotateLight();
        
    }
}
