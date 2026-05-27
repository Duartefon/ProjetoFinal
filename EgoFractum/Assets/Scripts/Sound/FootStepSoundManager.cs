using UnityEngine;

public class FootStepSoundManager : MonoBehaviour
{
    [Header("Footstep Sound Settings")]
    public float delayBetweenSteps;
    public AudioSource footStepsSource;
    public float velocityToTriggerStepSound;
    public float volume = 0.05f;
    public float earOffset = 0.25f;
    public float maxPitch = 1.5f;
    private float timeLastStep = 0;
    private float timestamp;

    [Header("Player")]
    public CharacterController player;

    private bool boolAux = false;
    private float panStereoAux = 1f;
    void Start()
    {
        footStepsSource.volume = volume;

        player = GetComponentInParent<CharacterController>();

        if (player == null)
        {
            Debug.LogError($"Não existe CharactterController no parent: {gameObject.transform.parent.name}");
        }
    }

    void Update()
    {
        timestamp += Time.deltaTime;

        if (player.velocity.magnitude > velocityToTriggerStepSound
            && timestamp > timeLastStep + delayBetweenSteps && player.isGrounded)
        {
            timeLastStep = Time.fixedTime;

            //Debug.Log($"timeStamp: {timestamp}, 
            // curenttime: {timeLastStep + delayBetweenSteps} ");

            footStepsSource.pitch = Random.Range(1, maxPitch);
            footStepsSource.volume = Random.Range(volume*0.5f, volume*1.2f);
            boolAux = !boolAux;
            footStepsSource.panStereo = boolAux ? -1 + earOffset : 1 - earOffset; // * (panStereoAux + earOffset);   
            footStepsSource.Play();

            //Debug.Log($"Velocity {player.velocity.magnitude } ");
            //   footStepsSource.panStereo =
        }
    }
}