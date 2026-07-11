using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KnobRotator : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private Transform linkedDial;
    [SerializeField] private int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;

    [Header("Audio")] [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rotationSound;

    [Header("Events")] public static Action<float> OnValueChanged;

    private XRBaseInteractor _interactor;
    private float _startAngle;
    private bool _requiresStartAngle = true;
    private bool _shouldGetHandRotation = false;
    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);

        if (audioSource != null) return;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = rotationSound;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.5f;
    }

    private void OnDisable()
    {
        grabInteractor.selectEntered.RemoveListener(GrabbedBy);
        grabInteractor.selectExited.RemoveListener(GrabEnd);
    }

    private void GrabEnd(SelectExitEventArgs arg0)
    {
        _shouldGetHandRotation = false;
        _requiresStartAngle = true;
    }

    private void GrabbedBy(SelectEnterEventArgs arg0)
    {
        _interactor = arg0.interactorObject as XRBaseInteractor;

        var directInteractor = arg0.interactorObject as XRDirectInteractor;
        if (directInteractor != null)
            directInteractor.hideControllerOnSelect = true;

        _shouldGetHandRotation = true;
        _startAngle = 0f;
    }

    void Update()
    {
        if (!_shouldGetHandRotation) return;

        var rotationAngle = GetInteractorRotation(); //pega na rotação do controller
        GetRotationDistance(rotationAngle);
    }

    private float GetInteractorRotation() => _interactor.GetComponent<Transform>().eulerAngles.z;

    private void GetRotationDistance(float currentAngle)
    {
        if (!_requiresStartAngle)
        {
            var angleDifference = Mathf.Abs(_startAngle - currentAngle);

            if (!(angleDifference > angleTolerance)) return;

            if (angleDifference > 270f) //para ver se ja passou do 0-360
            {
                float angleCheck;

                if (_startAngle < currentAngle)
                {
                    angleCheck = CheckAngle(currentAngle, _startAngle);

                    if (angleCheck > angleTolerance)

                    {
                        RotateDialClockwise();
                        _startAngle = currentAngle;
                    }
                }
                else if (_startAngle > currentAngle)
                {
                    angleCheck = CheckAngle(currentAngle, _startAngle);

                    if (angleCheck < angleTolerance)
                        return;
                    else
                    {
                        RotateDialAntiClockwise();
                        _startAngle = currentAngle;
                    }
                }
            }
            else
            {
                if (_startAngle < currentAngle)
                {
                    RotateDialAntiClockwise();
                    _startAngle = currentAngle;
                }
                else if (_startAngle > currentAngle)
                {
                    RotateDialClockwise();
                    _startAngle = currentAngle;
                }
            }
        }
        else
        {
            _requiresStartAngle = false;
            _startAngle = currentAngle;
        }
    }

    private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

    private void RotateDialClockwise()
    {
        linkedDial.Rotate(-snapRotationAmount, 0f, 0f, Space.Self);
        OnValueChanged?.Invoke(-snapRotationAmount);
        audioSource.PlayOneShot(rotationSound);
    }

    private void RotateDialAntiClockwise()
    {
        linkedDial.Rotate(snapRotationAmount, 0f, 0f, Space.Self);
        OnValueChanged?.Invoke(snapRotationAmount);
        audioSource.PlayOneShot(rotationSound);
    }
}