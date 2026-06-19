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
    public AudioClip normalStepSound, metalStepSound;

    [Header("Player")]
    public CharacterController player;
    private bool boolAux = false;

    void Start()
    {
        footStepsSource.volume = volume;
        player = GetComponentInParent<CharacterController>();
        if (player == null)
            Debug.LogError($"Não existe CharacterController no parent: {gameObject.transform.parent.name}");
    }

    void Update()
    {
        timestamp += Time.deltaTime;
        Debug.Log(player.velocity.magnitude);
        Debug.Log("Time: " + timestamp + "timeLastStep: " + timeLastStep + delayBetweenSteps + "Player grounded: " + player.isGrounded );
        float rayDistance = (player.height / 2f) - player.radius + 0.1f;
        Vector3 sphereOrigin = player.transform.position + player.center;
        bool isManuallyGrounded = Physics.SphereCast(sphereOrigin, player.radius, Vector3.down, out RaycastHit hit, rayDistance);
        if (player.velocity.magnitude > velocityToTriggerStepSound
            && timestamp > timeLastStep + delayBetweenSteps && isManuallyGrounded)
        {
            timeLastStep = Time.fixedTime;

            footStepsSource.clip = IsOnMetal() ? metalStepSound : normalStepSound;
            footStepsSource.pitch = Random.Range(1, maxPitch);
            footStepsSource.volume = Random.Range(volume * 0.5f, volume * 1.2f);
            boolAux = !boolAux;
            footStepsSource.panStereo = boolAux ? -1 + earOffset : 1 - earOffset;
            footStepsSource.Play();
        }
    }

    private bool IsOnMetal()
    {
        int metalLayer = LayerMask.NameToLayer("Metal");
        if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            bool onMetal = hit.collider.gameObject.layer == metalLayer;
            Debug.DrawRay(player.transform.position, Vector3.down * 2f, onMetal ? Color.yellow : Color.red);
            return onMetal;
        }

        Debug.DrawRay(player.transform.position, Vector3.down * 2f, Color.red);
        return false;
    }
}