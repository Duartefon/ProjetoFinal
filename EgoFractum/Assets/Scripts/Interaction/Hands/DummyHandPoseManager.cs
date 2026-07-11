using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class DummyHandPoseManager : MonoBehaviour
{
    // Temos de dar check a qual mão é que interagiu -> esquerda ou direita
    [Header("Dummy Hands")] [Tooltip("As dummy hands substituem a mão controlada pelo jogador.")] [SerializeField]
    private GameObject rightDummyHand;

    [SerializeField] private GameObject leftDummyHand;

    [Header("Player Hands (Visual)")] [SerializeField]
    private GameObject rightPlayerHand;

    [SerializeField] private GameObject leftPlayerHand;

    public bool isAnimatePosition = false;

    //debug
    public float radius = 0.5f;
    public float offset = 0.5f;
    public float rayLenght = 0.1f;

    public Transform[] rightHandRaycastOriginList;
    public Transform[] leftHandRaycastOriginList;


    public GameObject debugSphere;
    [SerializeField] private LayerMask grabbableMask;

    private void OnEnable()
    {
        var xrGrab = gameObject.GetComponent<XRGrabInteractable>();
        xrGrab.selectEntered.AddListener(OnGrab);
        xrGrab.selectExited.AddListener(OnUngrab);
    }

    private void OnDisable()
    {
        var xrGrab = gameObject.GetComponent<XRGrabInteractable>();

        xrGrab.selectEntered.RemoveListener(OnGrab);
        xrGrab.selectExited.RemoveListener(OnUngrab);
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        //dummyHand.SetActive(true);
        if (isAnimatePosition)
        {
            ShowHandPosition(arg0.interactorObject, arg0.interactableObject, true);
        }
        else
        {
            ShowHand(arg0.interactorObject, true);
        }


        //Debug.Log("Grabbed by: " + arg0.interactorObject);
    }

    private void OnUngrab(SelectExitEventArgs arg0)
    {
        ShowHand(arg0.interactorObject, false);
    }

    private void ShowHand(IXRSelectInteractor interactor, bool showDummy)
    {
        Transform transform = interactor?.transform;
        //Debug.Log("[SHOW HAND] Interactor: " + interactor);
        if (transform == null) return;

        bool isRight = transform.CompareTag("RightHand");
        bool isLeft = transform.CompareTag("LeftHand");

        //Debug.Log("Is Right? " + isRight + " | " + "Is Left? " +  isLeft);

        if (!isRight && !isLeft) return;

        if (isRight)
        {
            rightDummyHand.SetActive(showDummy);
            rightPlayerHand.SetActive(!showDummy);
        }
        else if (isLeft)
        {
            leftDummyHand.SetActive(showDummy);
            leftPlayerHand.SetActive(!showDummy);
        }
    }

    private void ShowHandPosition(IXRSelectInteractor interactor, IXRSelectInteractable interactable, bool showDummy)
    {
        var didHit = false;


        Transform transformInteractor = interactor?.transform;
        bool isRight = transformInteractor.CompareTag("RightHand");
        bool isLeft = transformInteractor.CompareTag("LeftHand");

        if (!transformInteractor || !isRight && !isLeft) return;

        if (isRight)
        {
            RaycastHit hit = new RaycastHit();
            foreach (var raycastOrigin in rightHandRaycastOriginList)
            {
                didHit = Physics.Raycast(raycastOrigin.position, -Vector3.up, out hit, rayLenght, grabbableMask);
                Debug.DrawRay(raycastOrigin.position, -Vector3.up * rayLenght, didHit ? Color.green : Color.red,
                    2f); // stays visible 2 seconds
                if (didHit) break;
            }

            if (!didHit) return;

            rightDummyHand.SetActive(showDummy);
            rightDummyHand.transform.position = new Vector3(hit.point.x, rightDummyHand.transform.position.y,
                rightDummyHand.transform.position.z);
            rightPlayerHand.SetActive(!showDummy);
        }

        if (isLeft)
        {
            RaycastHit hit = new RaycastHit();
            foreach (var raycastOrigin in leftHandRaycastOriginList)
            {
                didHit = Physics.Raycast(raycastOrigin.position, -Vector3.up, out hit, rayLenght, grabbableMask);
                Debug.DrawRay(raycastOrigin.position, -Vector3.up * rayLenght, didHit ? Color.green : Color.red,
                    2f);
                if (didHit) break;
            }

            leftDummyHand.SetActive(showDummy);
            rightDummyHand.transform.position = new Vector3(hit.point.x, leftDummyHand.transform.position.y,
                leftDummyHand.transform.position.z);
            leftPlayerHand.SetActive(!showDummy);
        }
    }
}