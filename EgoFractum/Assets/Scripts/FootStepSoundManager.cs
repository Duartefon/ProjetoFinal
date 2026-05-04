using UnityEngine;

public class FootStepSoundManager : MonoBehaviour
{
    public CharacterController player;
    public float delayBetweenSteps;
    public AudioSource footStepsSource;
    public float velocityToTriggerStepSound;

    public float earOffset = 0.25f;
    public float pitch = 0;
    private float timeLastStep = 0;
    private float timestamp;

    private bool boolAux = false;
    private float panStereoAux = 1f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timestamp += Time.deltaTime;
        if (player.velocity.magnitude > velocityToTriggerStepSound && timestamp > timeLastStep + delayBetweenSteps)
        {
            timeLastStep = Time.fixedTime;
            
            
            Debug.Log($"timeStamp: {timestamp}, curenttime: {timeLastStep + delayBetweenSteps} ");
            footStepsSource.pitch = Random.Range(1, pitch);
            boolAux = !boolAux;
            footStepsSource.panStereo = boolAux ?  -1+earOffset : 1-earOffset; // * (panStereoAux + earOffset);   
            footStepsSource.Play();
            
            
            Debug.Log($"Velocity {player.velocity.magnitude } ");
         //   footStepsSource.panStereo =
        }
        else
        {  
        }
 
        
        
        
    }
}