using UnityEngine;

public class FootStepSoundManager : MonoBehaviour
{
    [Header("Footstep Sound Settings")] 
    [SerializeField] private float delayBetweenSteps;
    [SerializeField] private AudioSource footStepsSource;
    [SerializeField] private float velocityToTriggerStepSound;
    [SerializeField] private float volume = 0.05f;
    [SerializeField] private float earOffset = 0.25f;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private LayerMask metalLayer;
    [SerializeField] private AudioClip normalStepSound, metalStepSound;

    [Header("Player")] 
    [SerializeField] private CharacterController player;

    private float _timeLastStep = 0;
    private float _timestamp;
    private bool _boolAux = false;

    void Start()
    {
        footStepsSource.volume = volume;
        player = GetComponentInParent<CharacterController>();
        if (player == null)
            Debug.LogError($"Não existe CharacterController no parent: {gameObject.transform.parent.name}");
    }

    /**
     * Verifica se o jogador está no chão (grounded) e toca o som de passos, alternando
     * entre o canal da direita e da esquerda para simular os passos reais. Cada passo, tem
     * um intervalo.
     */
    void FixedUpdate()
    {
        _timestamp += Time.deltaTime;
        var rayDistance = (player.height) - player.radius + 0.1f;
        var sphereOrigin = player.transform.position + player.center;
        
        var isManuallyGrounded =
            Physics.SphereCast(sphereOrigin, player.radius, Vector3.down, out RaycastHit hit, rayDistance);

        if (!(player.velocity.magnitude > velocityToTriggerStepSound)
            || !(_timestamp > _timeLastStep + delayBetweenSteps) || !isManuallyGrounded) return;
       
        _timeLastStep = Time.fixedTime;
        //footStepsSource.clip = 
        footStepsSource.pitch = Random.Range(1, maxPitch);
        footStepsSource.volume = Random.Range(volume * 0.5f, volume * 1.2f);
        _boolAux = !_boolAux;
        footStepsSource.panStereo = _boolAux ? -1 + earOffset : 1 - earOffset;
        footStepsSource.PlayOneShot( IsOnMetal() ? metalStepSound : normalStepSound);
    }

    /**
     * Verifica se a superfície (layer) onde o jogador se encontra é metal ou não.
     */
    private bool IsOnMetal()
    {
        var origin = player.transform.position;
        var direction = Vector3.down;
        var maxDistance = 0.5f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, metalLayer))
        {
            Debug.DrawRay(origin, direction, Color.yellow);
            return true;
        }

        Debug.DrawRay(origin, direction * maxDistance, Color.red);
        return false;
    }
}