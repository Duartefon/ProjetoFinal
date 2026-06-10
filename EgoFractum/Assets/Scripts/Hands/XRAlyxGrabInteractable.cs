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
                    _rbInteractable.linearVelocity = ComputeVelocity(); //Vector3.up*5f;
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
            var diff = _nearFarInteractor.transform.position - transform.position;
            var diffXZ = new Vector3(diff.x, 0, diff.z);
            var diffXZLength = diffXZ.magnitude;
            var diffYLength = diff.y;

            var angleInRadian = k_Jump_Angle * Mathf.Deg2Rad;

            var jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffXZLength, 2) /
                                         (2 * Mathf.Cos(angleInRadian) * Mathf.Cos(angleInRadian) *
                                          (diffXZ.magnitude * Mathf.Tan(angleInRadian) - diffYLength)));

            Vector3 jumpVelocity = diffXZ.normalized * (jumpSpeed * Mathf.Cos(angleInRadian))
                                   + Vector3.up * (jumpSpeed * Mathf.Sin(angleInRadian));
            return jumpVelocity;
        }
    }
}