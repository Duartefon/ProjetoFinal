using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

public class TransferBeam : MonoBehaviour
{
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private float _rayLength = 3f;
    [SerializeField] private LayerMask layerMask;
    public Material m;
    [SerializeField]  private GameObject miniPlayer;
    [SerializeField] private GameObject player;

    [SerializeField] TransitionEffectManager _effectManager;
    [SerializeField] private PlayerTransferData playerData;
    [SerializeField] private PlayerTransferData miniPlayerData;

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

    IEnumerator OnTransfer()
    {
        _effectManager.PlayEffect("playTransition");
        
        yield return new WaitForSeconds(_effectManager.effectTime*2);
        player.transform.position = miniPlayerData.position;
        player.transform.eulerAngles = miniPlayerData.rotation;
        player.transform.localScale = miniPlayerData.scale;

        player.GetComponent<CharacterController>().stepOffset = miniPlayerData.stepOffset;


    }
    
    
}
