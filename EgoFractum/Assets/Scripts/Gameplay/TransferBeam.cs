using System;
using System.Collections;
using UnityEngine;

public class TransferBeam : MonoBehaviour
{
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private float _rayLength = 3f;
    [SerializeField] private LayerMask layerMask;
    public Material m;
    [SerializeField]  private GameObject miniPlayer;
    [SerializeField] private GameObject player;

    [SerializeField]
    Animator _animator;
    

    private Coroutine _coroutine;
    private void Update()
    {
       var didHit=  Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward * _rayLength, out var hit, layerMask );
        Debug.DrawRay(_raycastOrigin.position, _raycastOrigin.forward * _rayLength);
        if (didHit && hit.transform.gameObject.CompareTag("Transfer"))
        {
            /*if (_coroutine) return;
            _coroutine = StartCoroutine(player)*/
        }

    }

    IEnumerator OnTransfer()
    {
       // _animator.Play();
        yield return null;
    }
    
    
}
