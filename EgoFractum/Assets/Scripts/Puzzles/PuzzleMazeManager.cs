using System;
using UnityEngine;

public class PuzzleMazeManager : MonoBehaviour
{
    [SerializeField]
    private bool puzzleStarted = false;
    [SerializeField]

    private EnemyStateMachine _enemyStateMachine;

    public void OnPuzzleStarted()
    {
        _enemyStateMachine.OnPuzzleStarted();
    }

    private void Update()
    {
        if(puzzleStarted)
            OnPuzzleStarted();
    }
}
