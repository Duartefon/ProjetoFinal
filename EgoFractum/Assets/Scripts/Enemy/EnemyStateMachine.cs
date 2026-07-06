using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyStates _currentState = EnemyStates.Idle;


    private bool puzzleStarted = false;
    private bool waitFinished = false;
    [SerializeField] private float waitTimeIdle = 3.5f;
    [SerializeField] private float enemyWalkSpeed = 0.05f;
    [SerializeField] private float enemyRunSpeed = 0.15f;
    
    public enum EnemyStates
    {
        Idle,
        Wander,
        Run
    }

    [SerializeField] private Animator _animator;
    
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float    _rayLength;

    private void Start()
    {
        _agent.speed = enemyWalkSpeed;
    }


    private void Update()
    {

        switch (_currentState)
        {
            case EnemyStates.Idle:
                UpdateIdleState();
                break;
            case EnemyStates.Wander:
                UpdateWanderState();
                break;
            case EnemyStates.Run:
                UpdateRunState();
                break;
            
        }
            
    }

    public void UpdateIdleState()
    {
        if (!puzzleStarted) return;
        if (waitFinished)
            _currentState = EnemyStates.Wander;
        UpdateAnimator("isIdle");


    }
    
    public void UpdateWanderState()
    {
        UpdateAgent(_agent, enemyWalkSpeed, _player.position);
        UpdateAnimator("isWalking");
        
        Physics.Raycast(_rayOrigin.position, transform.forward * _rayLength, out RaycastHit hit);
        
        Debug.DrawRay(_rayOrigin.position, transform.forward * _rayLength, Color.cornflowerBlue);
        if (hit.transform.gameObject.CompareTag("Player"))
            _currentState = EnemyStates.Run;

    }
    public void UpdateRunState()
    {
        UpdateAgent(_agent, enemyRunSpeed, _player.position);
        
        UpdateAnimator("isRunning");
        
        
    }
    /** Events
     *
     *
     * 
     **/

    public void OnPuzzleStarted()
    {
        if (puzzleStarted) return;
        
        puzzleStarted = true;
        StartCoroutine(WaitIdle(waitTimeIdle));
    }

    public IEnumerator WaitIdle(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        waitFinished = true;
    }


    private void UpdateAgent(NavMeshAgent agent ,float moveSpeed, Vector3 goalPosition)
    {
        agent.speed = moveSpeed;
        agent.destination = goalPosition;
    }
    private void UpdateAnimator(string triggerName)
    {
        //_animator.GetCurrentAnimatorStateInfo(-1);
        _animator.SetTrigger(triggerName);
    }
    
 

}
