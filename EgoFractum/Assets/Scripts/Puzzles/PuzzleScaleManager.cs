using System;
using UnityEngine;

public class PuzzleScaleManager : PuzzleManager
{

    [SerializeField] private DoorController[] doorsToUnlock;
    
    public void OnCompletePuzzle()
    {
        CompletePuzzle(puzzleName);
        foreach (var door in doorsToUnlock)
        {
            
            door.OpenWithoutGenerator();
            
        }
    }
    
    
    

 
}
