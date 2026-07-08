using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class HandFlashLight : MonoBehaviour
{
    [SerializeField] private Light light = null;

    [SerializeField] private int rechargeStep = 15;
    [SerializeField] private float rechargeCooldown = 0.5f;
    [SerializeField] private int unchargeStep = 15;
    [SerializeField] private float unchargeCooldown = 2.5f;

    
    [SerializeField] private float baseLightIntensity = 50f;
    [SerializeField] private float baseLightThreshold = 0.35f;

    //todo: convert to animation curve
    [SerializeField] private float lightBlinkingDecreaseStep = 0.2f;
    [SerializeField] private float baseLightInnerSpot = 23.9f;
    [SerializeField] private float baseLightOuterSpot = 56.7f;
    [SerializeField] [Range(0, 1)] private float blinkingTreshold = 0.35f;

    private float _timeStamp;
    private int _energy = 100;


    //Debug
    public TMP_Text text;

    private Coroutine _coroutine, _blinkingCoroutine, _chargeCoroutine;

    public InputActionReference flashlightAction;

    private FlashlightStates _currentState;

    private enum FlashlightStates
    {
        On,
        Off
    }

    public void OnEnable()
    {
        flashlightAction.action.performed += OnFlashlightButtonPress;
    }

    public void OnDisable()
    {
        flashlightAction.action.performed -= OnFlashlightButtonPress;
    }

    private void Start()
    {
        _currentState = FlashlightStates.Off;
    }


    public void LateUpdate()
    {
        switch (_currentState)
        {
            case FlashlightStates.On:

                if (Time.time - _timeStamp > unchargeCooldown)
                {
                    _timeStamp = Time.time;
                    _energy = Mathf.Clamp(_energy - unchargeStep, 0, 100);


                    if (_energy <= 100 * blinkingTreshold)
                    {
                        if (_blinkingCoroutine == null)
                            _blinkingCoroutine = StartCoroutine(BlinkingLight());
                    }

                    if (_energy <= 0)
                        _currentState = FlashlightStates.Off;
                }

                break;
            case FlashlightStates.Off:
                light.enabled = false;
                if (_blinkingCoroutine != null)
                    StopCoroutine(_blinkingCoroutine);

                if (Time.time - _timeStamp > rechargeCooldown)
                {
                    _timeStamp = Time.time;
                    _energy = Mathf.Clamp(_energy + rechargeStep, 0, 100);
                    light.intensity = Mathf.Clamp(light.intensity + rechargeStep, 0, baseLightIntensity);
                    ;
                }


                break;
        }
    }

    public void Update()
    {
        text.text =
            $"{_energy}%, isOn {light.enabled} ,Light Level{light.intensity}";
        Debug.Log($"{_energy}%, isOn {light.enabled} ,Light Level{light.intensity}");
    }

    private void OnFlashlightButtonPress(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Button pressed" + callbackContext.ReadValue<float>());
        light.enabled = !light.enabled;

        _currentState = light.enabled ? FlashlightStates.On : FlashlightStates.Off;
    }

    private IEnumerator BlinkingLight()
    {
        while (_currentState == FlashlightStates.On)
        {
            //Debug.Log(Mathf.Sin(Time.time) * 50);
            if (light.intensity >= baseLightIntensity * baseLightThreshold)
            {
                Debug.Log("I'm starting to decrease light ");
                light.intensity = Mathf.Clamp(light.intensity - lightBlinkingDecreaseStep, 0, baseLightIntensity);
            }
            else if (light.intensity >= 0)
            {
                //TODO: mudar isto para animation curves, salvo num scrip.obj
                light.intensity = 16 / 2f * Mathf.Sin(Time.time * 2) + 10;
                light.spotAngle = Mathf.Clamp(baseLightOuterSpot / 2 * Mathf.Sin(Time.time * 1 / 3) + 10, 25,
                    baseLightOuterSpot);
                light.innerSpotAngle = Mathf.Clamp(baseLightInnerSpot / 2 * Mathf.Sin(Time.time * 1 / 3) + 10, 5,
                    baseLightInnerSpot);
            }

            yield return null;
        }
    }
}