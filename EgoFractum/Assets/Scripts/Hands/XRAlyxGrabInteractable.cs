using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Hands
{
    
    public class XRAlyxGrabInteractable : XRGrabInteractable
    {
        public float minVel = 2f;

        private NearFarInteractor _nearFarInteractor;
        private Vector3 _previousPos;
        private bool _canJump = true;
        private const float k_Jump_Angle = 60f;

        private Rigidbody _rbInteractable;

        public float catchFlightTime = 0.4f; // tune to taste, or make it scale with distance
        protected override void Awake()
        {
            base.Awake();
            _rbInteractable = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_canJump && isSelected && firstInteractorSelecting is NearFarInteractor)
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

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (args.interactorObject is NearFarInteractor interactor)
            {
                trackPosition = false;
                trackRotation = false;
                throwOnDetach = false;

                _nearFarInteractor = interactor;
                _previousPos = _nearFarInteractor.transform.position;

                _canJump = true;
            } else {
                trackPosition = true;
                trackRotation = true;
                throwOnDetach = true;
            }

            base.OnSelectEntered(args);
        }

        private Vector3 ComputeVelocity()
        {
            Vector3 target = _nearFarInteractor.transform.position;
            Vector3 displacement = target - transform.position;

            // Optional: scale flight time with distance so far throws aren't instant
            // float t = Mathf.Clamp(displacement.magnitude / someSpeed, 0.2f, 0.8f);
            float t = catchFlightTime;

            // v = displacement/t - 0.5*g*t  (solves x(t) = x0 + v*t + 0.5*g*t^2 for v)
            Vector3 velocity = displacement / t - Physics.gravity * (0.5f * t);
            return velocity;
        }
    }
}