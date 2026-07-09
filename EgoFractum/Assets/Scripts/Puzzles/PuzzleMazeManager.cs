using System;
using UnityEngine;

public class PuzzleMazeManager : PuzzleManager
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private GameObject[] interactorsToDisable;
    public void OnPuzzleStarted()
    {
        _enemyStateMachine.OnPuzzleStarted();
        foreach (var gameObject in interactorsToDisable)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (puzzleStarted)
        {
            OnPuzzleStarted();
            
        }
    }
}