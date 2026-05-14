using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabHandPose : MonoBehaviour
{
    public HandData handPose;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 finalPosition;
    private Quaternion finalRotation;

    private Quaternion[] startFingerRotations;
    private Quaternion[] finalFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);

        handPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            SetHandDataValues(handData, handPose);
            SetHandData(handData, finalPosition, finalRotation, finalFingerRotations);
        }
    }

    public void SetHandDataValues(HandData hand1, HandData hand2) 
    {
        initialPosition = hand1.root.localPosition;
        initialRotation = hand1.root.localRotation;
        finalPosition = hand2.root.localPosition;
        finalRotation = hand2.root.localRotation;

        startFingerRotations = new Quaternion[hand1.fingers.Length];
        finalFingerRotations = new Quaternion[hand2.fingers.Length];

        for (int i = 0; i < hand1.fingers.Length; i++)
        {
            startFingerRotations[i] = hand1.fingers[i].localRotation;
            finalFingerRotations[i] = hand2.fingers[i].localRotation;
        }
    }

    public void SetHandData(HandData hand, Vector3 newPos, Quaternion newRot, Quaternion[] newFingerRots)
    {
        hand.root.localPosition = newPos;
        hand.root.localRotation = newRot;

        for (int i = 0; i < hand.fingers.Length; i++)
        {
            hand.fingers[i].localRotation = newFingerRots[i];
        }
    }

}


