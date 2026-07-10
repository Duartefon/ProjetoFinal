using System;
using System.Collections;
using System.Collections.Generic;
using KinoGlitch;
using PuzzleSystem;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyStateMachine : MonoBehaviour
{
    public EnemyStates currentState { get; private set; } = EnemyStates.Idle;

    private bool puzzleStarted = false;
    private bool waitFinished = false;
    [SerializeField] private float waitTimeIdle = 3.5f;
    [SerializeField] private float enemyWalkSpeed = 0.05f;
    [SerializeField] private float enemyRunSpeed = 0.15f;
    [SerializeField] private float enemyStunSpeed = 0.05f;
    [SerializeField] private float enemyTurnSpeed = 0.15f;
    

    [SerializeField] private ClockDelay internalClock;

    public enum EnemyStates
    {
        Idle,
        Wander,
        Run,
        Stunned
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _rayLength;

    [SerializeField] private float runDelay = 1f;
    private float timeStamp = 0f;
    private Vector3 _targetPosition;
    [SerializeField] private float wanderDelay = 2.25f;

    [SerializeField] private float attackDist = 2.25f;

    //TODO: this manager will be static 
    [SerializeField] private TransitionEffectManager _transitionEffectManager;
    //TODO: MazeManager has a ref to zombie, so zmbie will emit event player dead and puzzlem anager will do its logic from there

    [SerializeField] private PuzzleMazeManager _puzzleMazeManager;
    [SerializeField] private DigitalGlitchController _playerGlitchEffect;
    private float mapRadius = 0.5f;

    private bool _isPlayerDead = false;

    private void Start()
    {
        _agent.speed = enemyWalkSpeed;
        _agent.angularSpeed = enemyTurnSpeed;
    }

    private void Update()
    {
        if (!puzzleStarted) return;

        _agent.isStopped = false;
        
        //Debug.Log("CurrentState:  " + currentState);
        switch (currentState)
        {
            case EnemyStates.Idle:
                UpdateIdleState();
                break;
            case EnemyStates.Wander:
                UpdateWanderState();
                UpdateGLitchEffect();
                break;
            case EnemyStates.Run:
                UpdateRunState();
                UpdateGLitchEffect();
                break;
            case EnemyStates.Stunned:
                UpdateStunState();
                break;
        }
    }

    private void UpdateStunState()
    {
        //_agent.enabled = false;
       // _agent.isStopped = true;
       UpdateAgent(_agent,enemyStunSpeed, _player.position );
    }

    public void UpdateIdleState()
    {
        var animState = true;
        if (waitFinished)
        {
            currentState = EnemyStates.Wander;
            animState = false;
        }

        UpdateAnimator("isIdle", animState);
    }

    public void UpdateWanderState()
    {
        var animState = true;


        if (Time.time + timeStamp >= wanderDelay)
        {
            timeStamp = Time.time;

            var randomPointNextToPlayer = new Vector3(_player.transform.position.x + Random.Range(-0.5f, 0.5f),
                _player.transform.position.y, _player.transform.position.z + Random.Range(-0.5f, 0.5f));


            var randomPointInMap = RandomNavSphere(randomPointNextToPlayer, 0.5f, LayerMask.GetMask("Default"));
            _targetPosition = _player.position;
        }

        UpdateAgent(_agent, enemyWalkSpeed, _player.position);


        var didHit = Physics.Raycast(_rayOrigin.position, transform.forward * _rayLength, out RaycastHit hit);

        Debug.DrawRay(_rayOrigin.position, transform.forward * _rayLength,
            didHit ? Color.cornflowerBlue : Color.darkRed);
        if (hit.transform.gameObject.CompareTag("Player"))
        {
            currentState = EnemyStates.Run;
            animState = false;
        }

        UpdateAnimator("isWalking", animState);
    }

    private void UpdateGLitchEffect()
    {
        _transitionEffectManager.SetAnimator(false);
        var dist = Vector3.Distance(_player.position, transform.position);
        //var perc = 1 - dist
        var perc = (mapRadius - dist) / (mapRadius * 2);
        Debug.Log("Dist:" + dist + " percent: " + perc);
        _playerGlitchEffect.Intensity = perc;
    }

    public void UpdateRunState()
    {
        var animState = true;
        _agent.speed = enemyRunSpeed;
        UpdateAgent(_agent, enemyRunSpeed, _player.position);
        if (Time.time + timeStamp >= runDelay)
        {
            timeStamp = Time.time;

            _targetPosition = _player.position;

            if (Vector3.Distance(transform.position, _player.position) <= attackDist)
            {
                //TODO: all available transitions effects should be in the manager
                //this should be the maze manager responsibility, zombie only tells player dead

                _transitionEffectManager.PlayEffect("playTransition");

                Debug.Log("Player dead!");
            }
        }

        UpdateAgent(_agent, enemyWalkSpeed, _player.position);


        UpdateAnimator("isRunning", animState);
    }

    /** Events
     *
     *
     *
     **/
    public void OnLightStun(bool value)
    {
        currentState = value ? EnemyStates.Stunned : EnemyStates.Wander;
       
        Debug.Log("IM STUNNED: " + currentState);
    }

    public void OnPuzzleStarted()
    {
        if (puzzleStarted) return;

        puzzleStarted = true;
        StartCoroutine(WaitIdle(waitTimeIdle));
    }

    /** IEnumerators
   *
   *
   *
   **/
    public IEnumerator WaitIdle(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        waitFinished = true;
    }

    /** Helpers
   *
   *
   *
   **/
    private void UpdateAgent(NavMeshAgent agent, float moveSpeed, Vector3 goalPosition)
    {
        agent.speed = moveSpeed;
        agent.destination = goalPosition;
    }

    private void UpdateAnimator(string parameterName, bool value)
    {
        //_animator.GetCurrentAnimatorStateInfo(-1);
        _animator.SetBool(parameterName, value);
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}