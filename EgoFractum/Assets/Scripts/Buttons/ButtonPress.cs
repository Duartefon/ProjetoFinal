using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class ButtonPress : MonoBehaviour
{
    [Header("Elevator Reference")]
    public UnityEvent elevatorFunction;

    [Header("Button")]
    public Transform button;
    public Vector3 localAxis = Vector3.back;
    public float maxTravel = 0.015f; // em metros
    public float releaseSpeed = 5f;

    private Vector3 initialLocalPosition;
    private XRBaseInteractable interactable;
    private XRPokeInteractor activePoker;
    private bool hasInvoked;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Press);
        interactable.hoverExited.AddListener(Release);
        initialLocalPosition = button.localPosition;
    }

    private void Press(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor poker)
        {
            activePoker = poker;
            hasInvoked = false;
        }
    }

    private void Release(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            activePoker = null;
            hasInvoked = false;
        }
    }

    void Update()
    {
        if (activePoker != null)
        {
            Vector3 pokerLocalPos = button.parent != null
                ? button.parent.InverseTransformPoint(activePoker.attachTransform.position)
                : activePoker.attachTransform.position;

            Vector3 displacement = pokerLocalPos - initialLocalPosition;
            float travel = Vector3.Dot(displacement, localAxis.normalized);
            travel = Mathf.Clamp(travel, 0f, maxTravel);

            button.localPosition = initialLocalPosition + localAxis.normalized * travel;

            if (travel >= maxTravel && !hasInvoked)
            {
                hasInvoked = true;
                Debug.Log("Button fully pressed, invoking: " + elevatorFunction);
                elevatorFunction.Invoke();
            }
        }
        else
        {
            button.localPosition = Vector3.Lerp(
                button.localPosition,
                initialLocalPosition,
                Time.deltaTime * releaseSpeed
            );
        }
    }
}