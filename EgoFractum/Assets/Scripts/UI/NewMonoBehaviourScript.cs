using UnityEngine;

//Attach this script to the GameObject you would like to have mouse hovering detected on
//This script outputs a message to the Console when the mouse pointer is currently detected hovering over the GameObject and also when the pointer leaves.

using UnityEngine;
using UnityEngine.EventSystems;


public class Example : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Material teste;
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
        teste.SetFloat("_isSelected",1);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        teste.SetFloat("_isSelected",0);
        Debug.Log("Cursor Exiting " + name + " GameObject");
    }
}