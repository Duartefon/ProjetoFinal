using System.Collections;
using PuzzleSystem;
using ScriptableObjects;
using UnityEngine;

namespace Gameplay
{
    public class TransferBeam : MonoBehaviour
    {
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private float _rayLength = 3f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private GameObject player;

        [SerializeField] TransitionEffectManager _transitionEffectManager;
        [SerializeField] private PlayerTransferData playerData;
        [SerializeField] private PlayerTransferData miniPlayerData;
        [SerializeField] private PuzzleMazeManager _puzzleMazeManager;
        [SerializeField] private TransferVFXSettings _VFXSettings;
      
        private enum TransferState
        {
            PlayerToPuzzle,
            PuzzleToPlayer
        }
        private Coroutine _coroutine;
        private void Update()
        {
            bool didHit = Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward,
                out var hit, _rayLength, layerMask);
            Debug.DrawRay(_raycastOrigin.position, _raycastOrigin.forward * _rayLength);

            bool aimingAtTransfer = didHit && hit.transform.gameObject.CompareTag("Transfer");

            _VFXSettings.Tick(aimingAtTransfer, Time.deltaTime);

            if (_VFXSettings.IsFull && _coroutine == null)
                _coroutine = StartCoroutine(OnTransfer(TransferState.PlayerToPuzzle));
        }

        IEnumerator OnTransfer(TransferState transferState)
        {
            _transitionEffectManager.PlayEffect("playTransition");
            yield return new WaitForSeconds(_transitionEffectManager.effectTime*2);

            if (transferState.Equals(TransferState.PlayerToPuzzle))
            {
                _transitionEffectManager.TransitionPlayerTo(player.transform, miniPlayerData);
                _puzzleMazeManager.OnPuzzleStarted();
            }
            else if (transferState.Equals(TransferState.PuzzleToPlayer))
                _transitionEffectManager.TransitionPlayerTo(player.transform, miniPlayerData);

            StopAllCoroutines();
        }
        IEnumerator OnTransferReset( )
        {
            _transitionEffectManager.PlayEffect();
            yield return new WaitForSeconds(_transitionEffectManager.effectTime);
            
            _transitionEffectManager.TransitionPlayerToPositionRotation(player.transform, miniPlayerData);
            _puzzleMazeManager.OnPuzzleStarted();
           
            StopAllCoroutines();
        }


        public void OnResetPlayer()
        {
           
            StartCoroutine(OnTransferReset( ));
        }
        
        
        
        
        

 
    
    }
}
