using System;
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
    [SerializeField] private Transform raycastOrigin;

    private float _timeStamp;

    private int _energy = 100;

    //TODO: make a raycast emitter class
    [SerializeField] private float rayCastLenght;


    private EnemyStateMachine _enemyStateMachine;

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
                    DechargeLight();
                }

                var didHit = Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hit,
                    rayCastLenght , LayerMask.GetMask("Zombie"));
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * rayCastLenght,
                    didHit ? Color.green : Color.red);

                if (didHit)
                {
                    Debug.Log("I HIT: " + hit);
                    if (hit.transform.CompareTag("Zombie"))
                    {
                        _enemyStateMachine = hit.transform.GetComponent<EnemyStateMachine>();
                        _enemyStateMachine.OnLightStun(true);
                    }
                    
              
                }
                else  if(_enemyStateMachine)
                {
                    Debug.Log("Zombie is out of light");
                    _enemyStateMachine.OnLightStun(false);
                }
                
                

                break;
            case FlashlightStates.Off:
                flashLight.enabled = false;
                if (_blinkingCoroutine != null)
                    StopCoroutine(_blinkingCoroutine);

                if (Time.time - _timeStamp > rechargeCooldown)
                {
                    _timeStamp = Time.time;
                    ChargeLight();
                }

                break;
        }
    }

    public void Update()
    {
        text.text =
            $"{_energy}%, isOn {flashLight.enabled} ,Light Level{flashLight.intensity}";
        Debug.Log($"{_energy}%, isOn {flashLight.enabled} ,Light Level{flashLight.intensity}");
    }

    private void OnFlashlightButtonPress(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Button pressed" + callbackContext.ReadValue<float>());
        flashLight.enabled = !flashLight.enabled;

        _currentState = flashLight.enabled ? FlashlightStates.On : FlashlightStates.Off;
    }

    private void DechargeLight()
    {
        _energy = Mathf.Clamp(_energy - unchargeStep, 0, 100);

        if (_energy <= 100 * blinkingTreshold)
        {
            if (_blinkingCoroutine == null)
                _blinkingCoroutine = StartCoroutine(BlinkingLight());
        }


        if (_energy <= 0)
            _currentState = FlashlightStates.Off;
    }


    private void ChargeLight()
    {
        _energy = Mathf.Clamp(_energy + rechargeStep, 0, 100);
        flashLight.intensity = Mathf.Clamp(flashLight.intensity + rechargeStep, 0, baseLightIntensity);
    }

    private IEnumerator BlinkingLight()
    {
        while (_currentState == FlashlightStates.On)
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
                //TODO: mudar isto para animation curves, salvo num scrip.obj
                flashLight.intensity = 16 / 2f * Mathf.Sin(Time.time * 2) + 10;
                flashLight.spotAngle = Mathf.Clamp(baseLightOuterSpot / 2 * Mathf.Sin(Time.time * 1 / 3) + 10, 25,
                    baseLightOuterSpot);
                flashLight.innerSpotAngle = Mathf.Clamp(baseLightInnerSpot / 2 * Mathf.Sin(Time.time * 1 / 3) + 10, 5,
                    baseLightInnerSpot);
            }

            yield return null;
        }
    }
}