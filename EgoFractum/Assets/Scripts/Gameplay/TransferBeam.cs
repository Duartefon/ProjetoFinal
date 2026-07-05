using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Gameplay
{
    public class TransferBeam : MonoBehaviour
    {
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private float _rayLength = 3f;
        [SerializeField] private LayerMask layerMask;
        public Material m;
        [SerializeField] private GameObject player;

        [SerializeField] TransitionEffectManager _effectManager;
        [SerializeField] private PlayerTransferData playerData;
        [SerializeField] private PlayerTransferData miniPlayerData;

        private enum TransferState
        {
            PlayerToPuzzle,
            PuzzleToPlayer
        }
        private Coroutine _coroutine;
        private void Update()
        {
            var didHit=  Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward * _rayLength, out var hit, layerMask );
            Debug.DrawRay(_raycastOrigin.position, _raycastOrigin.forward * _rayLength);
            if (didHit && hit.transform.gameObject.CompareTag("Transfer"))
            {
                if (_coroutine != null) return;
                _coroutine = StartCoroutine(OnTransfer());
            }

        }

        IEnumerator OnTransfer(TransferState transferState)
        {
            _effectManager.PlayEffect("playTransition");
            yield return new WaitForSeconds(_effectManager.effectTime*2); 
        
            if(transferState.Equals(TransferState.PlayerToPuzzle))
                TransitionPlayerTo(miniPlayerData);
            else if (transferState.Equals(TransferState.PuzzleToPlayer))
                TransitionPlayerTo(playerData);


        }

        private void TransitionPlayerTo(PlayerTransferData plData)
        {
            player.transform.position = miniPlayerData.position;
            player.transform.eulerAngles = miniPlayerData.rotation;
            player.transform.localScale = miniPlayerData.scale;

            player.GetComponent<CharacterController>().stepOffset = miniPlayerData.stepOffset;
        }
    
    
    }
}
