using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleScaleManager : Puzzle
    {

        [SerializeField] private DoorController[] doorsToUnlock;
    
        public void OnCompletePuzzle()
        {
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            
            foreach (var door in doorsToUnlock)
            {
            
                door.OpenWithoutGenerator();
            
            }
        }
    
    
    

 
    }
}
