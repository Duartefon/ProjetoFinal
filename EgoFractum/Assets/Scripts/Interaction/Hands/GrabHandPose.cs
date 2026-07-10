using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabHandPose : MonoBehaviour
{
    public HandData handPose;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _finalPosition;
    private Quaternion _finalRotation;

    private Quaternion[] _startFingerRotations;
    private Quaternion[] _finalFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);

        handPose.gameObject.SetActive(false);
    }

    private void SetupPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            SetHandDataValues(handData, handPose);
            SetHandData(handData, _finalPosition, _finalRotation, _finalFingerRotations);
        }
    }

    private void SetHandDataValues(HandData hand1, HandData hand2) 
    {
        _initialPosition = hand1.root.localPosition;
        _initialRotation = hand1.root.localRotation;
        _finalPosition = hand2.root.localPosition;
        _finalRotation = hand2.root.localRotation;

        _startFingerRotations = new Quaternion[hand1.fingers.Length];
        _finalFingerRotations = new Quaternion[hand2.fingers.Length];

        for (int i = 0; i < hand1.fingers.Length; i++)
        {
            _startFingerRotations[i] = hand1.fingers[i].localRotation;
            _finalFingerRotations[i] = hand2.fingers[i].localRotation;
        }
    }

    private void SetHandData(HandData hand, Vector3 newPos, Quaternion newRot, Quaternion[] newFingerRots)
    {
        hand.root.localPosition = newPos;
        hand.root.localRotation = newRot;

        for (int i = 0; i < hand.fingers.Length; i++)
        {
            hand.fingers[i].localRotation = newFingerRots[i];
        }
    }

}


