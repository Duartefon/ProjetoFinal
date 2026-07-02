using System;
using Gameplay;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class KnobRotator : MonoBehaviour
{
    [SerializeField] Transform linkedDial;
    [SerializeField] private int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;
    [SerializeField] private GameObject RighthandModel;
    [SerializeField] private GameObject LefthandModel;
    [SerializeField] bool shouldUseDummyHands;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rotationSound;

    [Header("Events")]
    [SerializeField]
    public static Action<float> OnValueChanged;

    private XRBaseInteractor interactor;
    private float startAngle;
    private bool requiresStartAngle = true;
    private bool shouldGetHandRotation = false;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractor => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

    public VoltageScript voltageController;

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = rotationSound;
            audioSource.spatialBlend = 1f;
            audioSource.volume = 0.5f;
        }
    }
    private void OnDisable()
    {
        grabInteractor.selectEntered.RemoveListener(GrabbedBy);
        grabInteractor.selectExited.RemoveListener(GrabEnd);
    }

    private void GrabEnd(SelectExitEventArgs arg0)
    {
        shouldGetHandRotation = false;
        requiresStartAngle = true;
        HandModelVisibility(false);
    }

    private void GrabbedBy(SelectEnterEventArgs arg0)
    {
        interactor = arg0.interactorObject as XRBaseInteractor;

        var directInteractor = arg0.interactorObject as XRDirectInteractor;
        if (directInteractor != null)
            directInteractor.hideControllerOnSelect = true;

        shouldGetHandRotation = true;
        startAngle = 0f;
        HandModelVisibility(true);
    }

    private void HandModelVisibility(bool visibilityState)
    {
        if (!shouldUseDummyHands)
            return;

        if (interactor.CompareTag("RightHand"))
            RighthandModel.SetActive(visibilityState);
        else
            LefthandModel.SetActive(visibilityState);
    }

    void Update()
    {
        if (shouldGetHandRotation)
        {
            var rotationAngle = GetInteractorRotation(); //pega na rotação do controller
            GetRotationDistance(rotationAngle);
        }
    }

    public float GetInteractorRotation() => interactor.GetComponent<Transform>().eulerAngles.z;

    private void GetRotationDistance(float currentAngle)
    {
        if (!requiresStartAngle)
        {
            var angleDifference = Mathf.Abs(startAngle - currentAngle);

            if (angleDifference > angleTolerance)
            {
                if (angleDifference > 270f) //para ver se ja passou do 0-360
                {
                    float angleCheck;

                    if (startAngle < currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance)
                            return;
                        else
                        {
                            RotateDialClockwise();
                            startAngle = currentAngle;
                        }
                    }
                    else if (startAngle > currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance)
                            return;
                        else
                        {
                            RotateDialAntiClockwise();
                            startAngle = currentAngle;
                        }
                    }
                }
                else
                {
                    if (startAngle < currentAngle)
                    {
                        RotateDialAntiClockwise();
                        startAngle = currentAngle;
                    }
                    else if (startAngle > currentAngle)
                    {
                        RotateDialClockwise();
                        startAngle = currentAngle;
                    }
                }
            }
        }
        else
        {
            requiresStartAngle = false;
            startAngle = currentAngle;
        }
    }

    private float CheckAngle(float currentAngle, float _startAngle) => (360f - currentAngle) + _startAngle;

    private void RotateDialClockwise()
    {
        linkedDial.Rotate(snapRotationAmount, 0f, 0f, Space.Self);
        OnValueChanged?.Invoke(snapRotationAmount);
        audioSource.Play(); 
    }


    private void RotateDialAntiClockwise()
    {
        linkedDial.Rotate(-snapRotationAmount, 0f, 0f, Space.Self);
        OnValueChanged?.Invoke(snapRotationAmount);
        audioSource.Play();
    }
}



