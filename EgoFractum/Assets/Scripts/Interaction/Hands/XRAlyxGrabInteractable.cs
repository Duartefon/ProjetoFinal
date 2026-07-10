using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Hands
{
    public class XRAlyxGrabInteractable : XRGrabInteractable
    {
        public float minVel = 2f;
        public float catchFlightTime = 0.4f;

        // Should roughly match NearFarInteractor's near sphere-cast radius.
        // Anything grabbed within this distance is treated as a normal hand grab.
        public float nearGrabMaxDistance = 0.25f;

        private NearFarInteractor _nearFarInteractor;
        private Vector3 _previousPos;
        private bool _canJump = true;
        private bool _isFarGrab;

        private Rigidbody _rbInteractable;

        protected override void Awake()
        {
            base.Awake();
            _rbInteractable = GetComponent<Rigidbody>();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            Vector3 preservedVelocity = _rbInteractable.linearVelocity;
            Vector3 preservedAngularVelocity = _rbInteractable.angularVelocity;

            var nearFar = args.interactorObject as NearFarInteractor;
            float distance = nearFar != null
                ? Vector3.Distance(nearFar.transform.position, transform.position)
                : 0f;

            _isFarGrab = nearFar != null && distance > nearGrabMaxDistance;

            Debug.Log($"Grabbed by {args.interactorObject}, distance: {distance:F2}, isFarGrab: {_isFarGrab}");

            if (_isFarGrab)
            {
                trackPosition = false;
                trackRotation = false;
                throwOnDetach = false;

                _nearFarInteractor = nearFar;
                _previousPos = _nearFarInteractor.transform.position;
                _canJump = true;
            }
            else
            {
                trackPosition = true;
                trackRotation = true;
                throwOnDetach = true;
                _canJump = false;
            }

            base.OnSelectEntered(args);

            if (_isFarGrab)
            {
                _rbInteractable.linearVelocity = preservedVelocity;
                _rbInteractable.angularVelocity = preservedAngularVelocity;
            }
        }

        private void Update()
        {
            if (_canJump && _isFarGrab && isSelected && firstInteractorSelecting is NearFarInteractor)
            {
                Vector3 vel = (_nearFarInteractor.transform.position - _previousPos) / Time.deltaTime;
                _previousPos = _nearFarInteractor.transform.position;

                if (vel.magnitude > minVel)
                {
                    Drop();
                    _rbInteractable.linearVelocity = ComputeVelocity();
                    _canJump = false;
                }
            }
        }

        private Vector3 ComputeVelocity()
        {
            Vector3 target = _nearFarInteractor.transform.position;
            Vector3 displacement = target - transform.position;
            float t = catchFlightTime;
            return displacement / t - Physics.gravity * (0.5f * t);
        }
    }
}