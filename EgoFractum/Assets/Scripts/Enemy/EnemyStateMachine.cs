using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyStates _currentState = EnemyStates.Idle;


    private bool puzzleStarted = false;
    private bool waitFinished = false;
    [SerializeField] private float waitTimeIdle = 3.5f;
    public enum EnemyStates
    {
        Idle,
        Wander,
        Run
    }

    
    
   
    
    
    
    
    private void Update()
    {
        Debug.Log("Im in the: " + _currentState);

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



    }
    
    public void UpdateWanderState()
    {
        
    }
    public void UpdateRunState()
    {
        
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
    
    
}
