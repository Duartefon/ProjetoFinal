using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class ButtonPress : MonoBehaviour
{
    [Header("Button Action")]
    public UnityEvent buttonAction;

    [Header("Button")]
    [SerializeField] private Transform button;
    [SerializeField] private Vector3 localAxis = Vector3.back;
    [SerializeField] private float maxTravel = 0.015f; // em metros
    [SerializeField] private float releaseSpeed = 5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonPressSound;

    private Vector3 _initialLocalPosition;
    private XRBaseInteractable _interactable;
    private XRPokeInteractor _activePoker;
    private bool _hasInvoked;

    void Start()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        _interactable.hoverEntered.AddListener(Press);
        _interactable.hoverExited.AddListener(Release);
        _initialLocalPosition = button.localPosition;
    }

    private void Press(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor poker)
        {
            _activePoker = poker;
            _hasInvoked = false;
        }
    }

    private void Release(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            _activePoker = null;
            _hasInvoked = false;
        }
    }

    private void PlayFeedbackSound()
    {
        if (audioSource != null && buttonPressSound != null) audioSource.PlayOneShot(buttonPressSound);
    }

    void Update()
    {
        if (_activePoker != null)
        {
            Vector3 pokerLocalPos = button.parent != null
                ? button.parent.InverseTransformPoint(_activePoker.attachTransform.position)
                : _activePoker.attachTransform.position;

            Vector3 displacement = pokerLocalPos - _initialLocalPosition;
            float travel = Vector3.Dot(displacement, localAxis.normalized);
            travel = Mathf.Clamp(travel, 0f, maxTravel);

            button.localPosition = _initialLocalPosition + localAxis.normalized * travel;

            if (travel >= maxTravel && !_hasInvoked)
            {
                _hasInvoked = true;
                PlayFeedbackSound();
                Debug.Log("Button fully pressed, invoking: " + buttonAction);
                buttonAction.Invoke();
            }
        }
        else
        {
            button.localPosition = Vector3.Lerp(
                button.localPosition,
                _initialLocalPosition,
                Time.deltaTime * releaseSpeed
            );
        }
    }
}