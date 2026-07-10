using NUnit.Framework.Internal;
using UnityEngine;
using EnemyStates = EnemyStateMachine.EnemyStates;
public class ZombieSoundManager : MonoBehaviour
{
    public ZombieData zombieData;
    private AudioSource audioSource;
    private AudioClip audioClip;
    
    // Reference to the specific AI on THIS object
    private float timer = 0f; 
    private float delayBetweenAudios = 8f;
    [SerializeField] 
    private EnemyStateMachine _stateMachine;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Play one random sound at spawn
        if(Random.value > 0.5f) 
             audioSource.PlayOneShot(GetRandomZombieSound(zombieData.walkSound));
    }

    

    void Update()
    {
        audioClip = null;
        //if (!zombie.IsAlive) return;
        timer -= Time.deltaTime;
        var currentState = _stateMachine.currentState;
        Debug.Log($"CurrentState {currentState}");

        switch (currentState)
        {
            case EnemyStates.Idle:
                return;
            case EnemyStates.Run:
                audioClip = GetRandomZombieSound(zombieData.runningSound);
                delayBetweenAudios = 3f; // Added a delay so they don't spam run sounds
                break;
            case EnemyStates.Wander:
                audioClip = GetRandomZombieSound(zombieData.walkSound);
                delayBetweenAudios = 5f; // Explicitly set delay for wandering
                break;
            default:
                return;
        }

        if (timer <= 0)
        {
            Debug.Log("Cheguei ao timer");
            if (audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
            timer = delayBetweenAudios;
        }
    }

    public AudioClip GetRandomZombieSound(AudioClip[] sounds)
    {
        if (sounds == null || sounds.Length == 0) return null;
        return sounds[Random.Range(0, sounds.Length)];
    }
}