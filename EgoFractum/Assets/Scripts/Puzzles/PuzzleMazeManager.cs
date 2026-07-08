using System;
using UnityEngine;

public class PuzzleMazeManager : PuzzleManager
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;

    public void OnPuzzleStarted()
    {
        _enemyStateMachine.OnPuzzleStarted();
    }

    private void Update()
    {
        if (puzzleStarted)
            OnPuzzleStarted();
    }
}