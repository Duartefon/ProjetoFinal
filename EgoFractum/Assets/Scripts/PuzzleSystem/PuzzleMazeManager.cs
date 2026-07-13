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
        [SerializeField] private IntroSequenceManager _introSequenceManager;

        [SerializeField] private DoorController _doorController;
        [SerializeField] private GameObject _miniPlayerModel;

        //for debbuging to delete
        public bool resetPuzzle = false;

        public void OnPuzzleStarted()
        {
            _enemyStateMachine.OnPuzzleStarted();
            _miniPlayerModel.SetActive(false);
            DisableInteractorComponents();
            _introSequenceManager.OnPlayMazeSequence();
            
            UnlockDoors();
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
            if (!_introSequenceManager.gameObject.activeSelf) _introSequenceManager.gameObject.SetActive(true);


            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            
            
            _introSequenceManager.StartEnding();
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
                _doorController.OpenWithoutGenerator();
            }
        }
    }
}