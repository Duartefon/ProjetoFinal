using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class IntroSequenceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image blackScreen;
    [FormerlySerializedAs("quoteText")] public TextMeshProUGUI introConsoleText;
    public TextMeshProUGUI introHudText;

    [Header("Audio")]
    public AudioSource introAudio; //I'm thinking this should be the sound of the different machines. Could also make a seperate sound manager that will be told by this manager to start played the sounds

    [Header("Player Controllers")]
    [SerializeField] private  GameObject locomotion; 
     

  //Trying out hiding the hands at the start of the intro
    public GameObject[] leftHandModel;
    public GameObject[] rightHandModel;

    [Header("Timings")]
    public float quoteStayTime = 6.5f;
    public float fadeSpeed = 1f;
    [SerializeField] private PlayerTransferData introPosition;
    
    [SerializeField] private PlayerTransferData introEndPosition;
    [SerializeField] private PlayerTransferData endingEndPosition;
    [SerializeField] private TransitionEffectManager _transitionEffectManager;
    private GameObject player;
    void Start()
    {
 
        introConsoleText.enabled = false;
        
        player = GameObject.FindGameObjectWithTag("Player");
        _transitionEffectManager.TransitionPlayerTo(player.transform, introPosition);
        
       // StartIntro(); 
    }

    public void StartIntro()
    {
        StartCoroutine(PlayIntroSequence(introPosition));
    }
    
    public void StartEnding()
    {
        StartCoroutine(PlayIntroSequence(endingEndPosition));
    }

    private IEnumerator PlayIntroSequence(PlayerTransferData positionData )
    {
          
        Debug.Log("I'M starting to fade");
        LockMovement(true);
        
        DisableHand(leftHandModel, true);
        DisableHand(rightHandModel, true);
 
        
        introConsoleText.enabled = true;
        introAudio.Play();
        yield return new WaitForSeconds(3f);
    
        yield return StartCoroutine(FadeText(introConsoleText, 1f));
        yield return new WaitForSeconds(quoteStayTime);
        yield return StartCoroutine(FadeText(introConsoleText, -0.01f));
     

        _transitionEffectManager.PlayEffect();
        
        yield return new WaitForSeconds(_transitionEffectManager.effectTime);
        
        
        
    
        
        _transitionEffectManager.TransitionPlayerTo(player.transform, positionData);
        
        DisableHand(leftHandModel, false);
        DisableHand(rightHandModel, false);
        introAudio.Stop();
        introHudText.enabled = true;
        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(4.5f);
        introHudText.enabled = false;
        
        LockMovement(false);
        Debug.Log("Intro finished. Waiting for player to open the door...");
        
        
    }

    private void DisableHand(GameObject[] hand, bool active)
    {
        foreach (var handObject in hand)
        {
            handObject.SetActive(!active);
        }
            
    }
    //Old version
    private IEnumerator PlayTextIntroSequence()
    {
        
        Debug.Log("I'M starting to fade");
        LockMovement(true);
    /*     leftHandModel.SetActive(false);
         
        rightHandModel.SetActive(false);*/
        introConsoleText.enabled = true;
        yield return new WaitForSeconds(3f);
    
        yield return StartCoroutine(FadeText(introConsoleText, 1f));
        yield return new WaitForSeconds(quoteStayTime);
        yield return StartCoroutine(FadeText(introConsoleText, -0.01f));
     

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeUI(blackScreen, 0f));
        //introAudio.Play();
        
        
        
    
        
        _transitionEffectManager.PlayEffect("playTransition");
        
     
        
        Debug.Log("Intro finished. Waiting for player to open the door...");
    }


    public void UnlockFinalMovement()
    {
        LockMovement(false);
        Debug.Log("Door opened! Player can now move freely.");
    }

    private void LockMovement(bool lockState)
    {
        locomotion.SetActive(!lockState);
    }

    private IEnumerator FadeUI(Image img, float targetAlpha)
    {
       
        Color c = img.color;
        while (Mathf.Abs(c.a - targetAlpha) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            img.color = c;
            Debug.Log($"imgColorTarget: {c.a} imgColorActual {img.color.a}");
            yield return null;
        }
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float targetAlpha)
    {
        Color c = text.color;
        while (Mathf.Abs(c.a - targetAlpha) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            text.color = c;
            yield return null;
        }
    }
}