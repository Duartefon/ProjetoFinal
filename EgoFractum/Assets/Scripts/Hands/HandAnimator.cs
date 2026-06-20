using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{
    
    public InputActionReference gripPressAction;

    public InputActionReference triggerPressAction;
    public InputActionReference thumbPressAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator handAnimator;
    public Dictionary<String, HandPose> HandPoses = new Dictionary<string, HandPose>
    {
        { "Grip", null },
        { "Pointing", null },
        { "Idle", null },
        

    };
    
    public enum Fingers
    {
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky
    }
    


    private void Update()
    {
 
            foreach (var finger in Enum.GetNames(typeof(Fingers)))
            {
                if (finger is nameof(Fingers.Thumb))
                {
                    Debug.Log($"I'm pressing trigger {triggerPressAction.action.ReadValue<float>()} force");
                    handAnimator.SetFloat(finger, thumbPressAction.action.ReadValue<float>());
                    continue;
                }

                if (finger is nameof(Fingers.Index))
                {
                    Debug.Log($"I'm pressing trigger {triggerPressAction.action.ReadValue<float>()} force");
                    handAnimator.SetFloat(finger, triggerPressAction.action.ReadValue<float>());
                    continue;
                }
                
                if (gripPressAction.action.ReadValue<float>() != 0)
                {
                    handAnimator.SetFloat(finger, gripPressAction.action.ReadValue<float>());
                    continue;
                }
                
 
                handAnimator.SetFloat(finger, 0);
               
                
            }

  

    }
}
