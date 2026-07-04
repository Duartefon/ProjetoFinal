using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public String mainLevel = "MainLevel";
    private Material lastButtonSelected = null;
    private Animator _cameraAnimator;

    private void Start()
    {
        _cameraAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    public void OnPlayButtonPressed()
    {
        StartCoroutine(goToMenu());
    }
//TODO: clean up this , distinguish between first play and load
    IEnumerator goToMenu()
    {
       _cameraAnimator.SetTrigger("playGlitch");
       yield return new WaitForSeconds(2.1f);
       
       
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
