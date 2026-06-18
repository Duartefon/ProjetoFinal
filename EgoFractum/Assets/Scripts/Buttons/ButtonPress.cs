using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ButtonPress : MonoBehaviour
{
    public Transform button;
    public Vector3 localAxis;
    public float releaseSpeed = 10f;
    private Vector3 offset;
    private Transform attachPoint;
    private Vector3 initialPosition;
    private XRBaseInteractable interactable;
    private bool isPressing;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Press);
        interactable.hoverExited.AddListener(Release);

        interactable.selectEntered.AddListener(Pressed);

        initialPosition = button.localPosition;
    }

    public void Press(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor pokeInteractor = (XRPokeInteractor)hover.interactorObject;
            isPressing = true;
            attachPoint = pokeInteractor.attachTransform;
            offset = button.position - attachPoint.position;
        }
    }

    private void Release(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isPressing = false;
        }
    }

    private void Pressed(SelectEnterEventArgs press)
    {
        if (press.interactorObject is XRPokeInteractor)
        {
            // faz um som de click
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressing)
        {
            // para o bitao so mexer num eixo
            Vector3 localBottunPos = button.InverseTransformDirection(attachPoint.position + offset);
            Vector3 constrainedLocalButtonPos = Vector3.Project(localBottunPos, localAxis);
            button.position = button.TransformPoint(constrainedLocalButtonPos);
        }
        else
        {
            button.localPosition = Vector3.Lerp(button.localPosition, initialPosition, Time.deltaTime * releaseSpeed);
        }
    }
}
