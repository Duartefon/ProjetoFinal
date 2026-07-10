using Gameplay;
using ScriptableObjects;
using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleMazeManager : Puzzle
    {
        [SerializeField] private EnemyStateMachine _enemyStateMachine;
        [SerializeField] private GameObject[] interactorsToDisable;
        [SerializeField] private TransferBeam _transferBeam;
        
        //for debbuging to delete
        public bool resetPuzzle = false;
        public void OnPuzzleStarted()
        {
            _enemyStateMachine.OnPuzzleStarted();
            DisableInteractorComponents();
        }

        public void OnPuzzleReset()
        {
            Reset();
            DisableInteractorComponents();

        }

        private void Reset()
        {
            _enemyStateMachine.OnPuzzleRestarted();
            resetPuzzle = false;
            puzzleStarted = false;
                    
                    
            _transferBeam.OnResetPlayer();
        }
        public void OnPuzzleEnded()
        {
            _enemyStateMachine.OnPuzzleRestarted();
        }

        private void DisableInteractorComponents()
        {
            foreach (var interactor in interactorsToDisable)
            {
                interactor.SetActive(false);
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
}