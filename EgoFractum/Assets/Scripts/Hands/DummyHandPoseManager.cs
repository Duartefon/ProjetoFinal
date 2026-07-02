using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class DummyHandPoseManager : MonoBehaviour
{
    // Temos de dar check a qual mão é que interagiu -> esquerda ou direita
    [Header("Dummy Hands")]
    [Tooltip("As dummy hands substituem a mão controlada pelo jogador.")]
    [SerializeField] private GameObject rightDummyHand;
    [SerializeField] private GameObject leftDummyHand;

    [Header("Player Hands (Visual)")]
    [SerializeField] private GameObject rightPlayerHand;
    [SerializeField] private GameObject leftPlayerHand;

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
        ShowHand(arg0.interactorObject, true);

        Debug.Log("Grabbed by: " + arg0.interactorObject);
    }

    private void OnUngrab(SelectExitEventArgs arg0)
    {
        ShowHand(arg0.interactorObject, false);
    }

    private void ShowHand(IXRSelectInteractor interactor, bool showDummy)
    {
        Transform transform = interactor?.transform;
        Debug.Log("[SHOW HAND] Interactor: " + interactor);
        if (transform == null) return;
        
        bool isRight = transform.CompareTag("RightHand");
        bool isLeft = transform.CompareTag("LeftHand");
        
        Debug.Log("Is Right? " + isRight + " | " + "Is Left? " +  isLeft);
        
        if(!isRight && !isLeft) return;

        if (isRight)
        {
            rightDummyHand.SetActive(showDummy);
            rightPlayerHand.SetActive(!showDummy);
        } else if (isLeft)
        {
            leftDummyHand.SetActive(showDummy);
            leftPlayerHand.SetActive(!showDummy);
        }
    }
}
