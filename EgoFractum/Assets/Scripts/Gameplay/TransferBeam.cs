using System;
using UnityEngine;

public class TransferBeam : MonoBehaviour
{
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private float _rayLength = 3f;
    [SerializeField] private LayerMask layerMask;
    public Material m;
    private void Update()
    {
       var didHit=  Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward * _rayLength, out var hit, layerMask );
        Debug.DrawRay(_raycastOrigin.position, _raycastOrigin.forward * _rayLength);
        if (didHit && hit.transform.gameObject.CompareTag("Transfer"))
            hit.transform.gameObject.GetComponent<Renderer>().material = m;

    }
}
