using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HandFlashLight : MonoBehaviour
{
    private bool _isLightOn = false;

    [FormerlySerializedAs("light")] [SerializeField]
    private Light flashLight = null;

    private int _energy = 100;
    [SerializeField] private int rechargeStep = 15;
    [SerializeField] private float rechargeCooldown = 0.5f;
    [SerializeField] private int unchargeStep = 15;
    [SerializeField] private float unchargeCooldown = 2.5f;
    [SerializeField] private float baseLightIntensity = 50f;
    [SerializeField] private float baseLightThreshold = 0.35f;
    private float _timeStamp;

    [SerializeField] private float lightBlinkingDecreaseStep = 0.2f;
    [SerializeField] private float baseLightInnerSpot = 23.9f;
    [SerializeField] private float baseLightOuterSpot = 56.7f;


    [SerializeField] [Range(0, 1)] private float blinkingTreshold = 0.35f;

    //Debug
    public TMP_Text text;

    private Coroutine _coroutine, _blinkingCoroutine, _chargeCoroutine;

    public InputActionReference flashlightAction;

    public void OnEnable()
    {
        flashlightAction.action.performed += OnFlashlightButtonPress;
    }

    public void OnDisable()
    {
        flashlightAction.action.performed -= OnFlashlightButtonPress;
    }

    public void Update()
    {
        text.text =
            $"{_energy}%, isOn {_isLightOn} ,Light Level{flashLight.intensity}";
    }

    IEnumerator FlashlightOn()
    {
        _timeStamp = Time.time;

        do
        {
            if (Time.time - _timeStamp > unchargeCooldown)
            {
                _timeStamp = Time.time;
                _energy = Mathf.Clamp(_energy - unchargeStep, 0, 100);

                if (_energy <= 100 * blinkingTreshold)
                {
                    if (_blinkingCoroutine == null)
                        _blinkingCoroutine = StartCoroutine(BlinkingLight());
                }
            }

            yield return null;
        } while (_energy > 0);
    }

    private IEnumerator BlinkingLight()
    {
        Debug.Log("I got Called");
        while (_isLightOn)
        {
            //Debug.Log(Mathf.Sin(Time.time) * 50);
            if (flashLight.intensity >= baseLightIntensity * baseLightThreshold)
            {
                Debug.Log("I'm starting to decrease light ");
                flashLight.intensity =
                    Mathf.Clamp(flashLight.intensity - lightBlinkingDecreaseStep, 0, baseLightIntensity);
            }
            else if (flashLight.intensity >= 0)
            {
                flashLight.intensity = 16 / 2f * Mathf.Sin(Time.time * 2) + 10;
                flashLight.spotAngle = Mathf.Clamp(baseLightOuterSpot / 2 * Mathf.Sin(Time.time * 1 / 3) + 10, 25,
                    baseLightOuterSpot);
                flashLight.innerSpotAngle = Mathf.Clamp(baseLightInnerSpot / 2 * Mathf.Sin(Time.time * 1 / 3) + 10, 5,
                    baseLightInnerSpot);
            }

            yield return null;
        }
    }

    private void OnFlashlightButtonPress(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Button pressed" + callbackContext.ReadValue<float>());
        flashLight.enabled = !flashLight.enabled;
        if (flashLight.enabled)
        {
            _isLightOn = true;
            _coroutine = StartCoroutine(FlashlightOn());
            if (_chargeCoroutine != null) StopCoroutine(ChargeLight());
        }
        else if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _isLightOn = false;
            _chargeCoroutine = StartCoroutine(ChargeLight());
            StopCoroutine(BlinkingLight());
            _blinkingCoroutine = null;
        }
    }

    IEnumerator ChargeLight()
    {
        flashLight.enabled = false;

        _isLightOn = false;
        do
        {
            if (Time.time - _timeStamp > rechargeCooldown)
            {
                _timeStamp = Time.time;
                _energy = Mathf.Clamp(_energy + rechargeStep, 0, 100);
                flashLight.intensity = Mathf.Clamp(flashLight.intensity + rechargeStep, 0, baseLightIntensity);
            }

            yield return null;
        } while (_energy < 100);
    }
}