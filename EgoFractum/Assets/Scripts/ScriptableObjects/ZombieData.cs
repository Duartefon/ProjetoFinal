using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    [SerializeField] public float runSpeed = 4.5f;
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] public float attackRate = 1.5f; // Seconds between hits
    [SerializeField] public float attackRange = 2.0f; // Seconds between hits
    [SerializeField] public float biteRange = 1f; // Seconds between hits
    

    [SerializeField] public AudioClip[] idleSound; 
    [SerializeField] public AudioClip[] walkSound;
    [SerializeField] public AudioClip[] runningSound; 
    [SerializeField] public AudioClip[] attackSound;
    [SerializeField] public AudioClip[] gettingHitsound;
    [SerializeField] public AudioClip[] attackingDeadPlayer;
}
