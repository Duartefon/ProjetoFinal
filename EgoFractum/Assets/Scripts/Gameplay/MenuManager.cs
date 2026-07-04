using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public String mainLevel = "MainLevel";
    private Material lastButtonSelected = null;
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(mainLevel);
    }

    public void OnLoadButtonPressed()
    {
        // evento para ver se a scene ja carregou
        SceneManager.sceneLoaded += OnSceneFullyLoaded;
        SceneManager.LoadScene(mainLevel);
    }

    private void OnSceneFullyLoaded(Scene scene, LoadSceneMode mode)
    {
        // unsubscribe ao evento
        SceneManager.sceneLoaded -= OnSceneFullyLoaded;
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.LoadGame();
        }
    }

    public void OnButtonSelected(Material buttonMaterial)
    {
        if(lastButtonSelected)
            lastButtonSelected.SetFloat("_isSelected", 0);
        buttonMaterial.SetFloat("_isSelected", 1);
        
        
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    
}
