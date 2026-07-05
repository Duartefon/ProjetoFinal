using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TransitionEffectManager _transitionEffectManager;
    private static readonly int IsSelected = Shader.PropertyToID("_isSelected");
    private Material lastButtonSelected = null;
   
    public String mainLevel = "MainLevel";

    public void OnPlayButtonPressed()
    {
        StartCoroutine(goToMenu());
    }
    
    //TODO: clean up this , distinguish between first play and load
    IEnumerator goToMenu()
    {
        _transitionEffectManager.PlayEffect();
        yield return new WaitForSeconds(_transitionEffectManager.effectTime);
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
            lastButtonSelected.SetFloat(IsSelected, 0);
        buttonMaterial.SetFloat(IsSelected, 1);
    }
    
    public void OnQuit()
    {
        Application.Quit();
    }
    
}
