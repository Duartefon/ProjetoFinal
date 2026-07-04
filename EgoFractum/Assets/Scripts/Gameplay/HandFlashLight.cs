using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandFlashLight : MonoBehaviour
{
    private bool _isActive = false;
    [SerializeField]
    private Light _light = null;

    private int _energy = 100;
    [SerializeField]
    
    private int rechargeStep = 15;
    [SerializeField]
    private float rechargeCooldown = 0.5f;
    [SerializeField]
    
    private int unchargeStep = 15;
    [SerializeField]
    private float unchargeCooldown = 2.5f;
    
    private float timeStamp;

    [SerializeField] [Range(0, 1)] private float blinkingTreshold = 0.35f;
    //Debug
    public TMP_Text text;

    private Coroutine _coroutine, _blinkingCoroutine;
    
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
            $"{_energy}%, Light Level{_light.intensity}";
    }

    IEnumerator FlashlightOn()
    {
        timeStamp = Time.time;

        do
        {    
            if (Time.time - timeStamp > unchargeCooldown)
            {

                timeStamp = Time.time;
                _energy = Mathf.Clamp(_energy-unchargeStep,0, 100);

                if (_energy <= 100 * blinkingTreshold)
                {
                    if(_blinkingCoroutine == null)
                        _blinkingCoroutine = StartCoroutine(BlinkingLight());
                }
            }
            yield return null;
            
        } while (_energy > 0);

 
    }

    IEnumerator BlinkingLight()
    {
        Debug.Log("I got Called");
        while (_isActive)
        {
            Debug.Log(Mathf.Sin(Time.time) * 50);
            _light.intensity +=  Mathf.Sin(Time.time) * 50;
            yield return null;
            
        }
    }
 
    public void OnFlashlightButtonPress(InputAction.CallbackContext callbackContext)
    {
       
        Debug.Log("Button pressed" +  callbackContext.ReadValue<float>());
        _light.enabled = !_light.enabled;
        if (_light.enabled)
            _coroutine =  StartCoroutine(FlashlightOn());
        else if(_coroutine != null)
            StopCoroutine(_coroutine);
        
            

    }

    
    
}
