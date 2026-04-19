using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public String mainLevel = "MainLevel";
  
    public void OnPlayButtonPressed()
    {

        SceneManager.LoadScene(mainLevel);
    }


    public void OnQuit()
    {
        Application.Quit();
    }
    
}
