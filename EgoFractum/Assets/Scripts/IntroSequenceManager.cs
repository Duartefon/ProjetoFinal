using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class IntroSequenceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image blackScreen;
    public TextMeshProUGUI quoteText;

    [Header("Audio")]
    public AudioSource introAudio; //I'm thinking this should be the sound of the different machines. Could also make a seperate sound manager that will be told by this manager to start played the sounds

    [Header("Player Controllers")]
    public ActionBasedContinuousMoveProvider moveProvider; 
    public ActionBasedContinuousTurnProvider turnProvider; 

  //Trying out hiding the hands at the start of the intro
    public GameObject leftHandModel;
    public GameObject rightHandModel;

    [Header("Timings")]
    public float quoteStayTime = 4f;
    public float fadeSpeed = 1f;

    void Start()
    {
 

         StartIntro(); 
    }

    public void StartIntro()
    {
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        LockMovement(true);
        leftHandModel.SetActive(false);
        rightHandModel.SetActive(false);
        yield return StartCoroutine(FadeUI(blackScreen, 255f));

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeText(quoteText, 1f));
        yield return new WaitForSeconds(quoteStayTime);
        yield return StartCoroutine(FadeText(quoteText, 0f));

        yield return new WaitForSeconds(1f);
        //introAudio.Play();
        

        yield return StartCoroutine(FadeUI(blackScreen, 1f));

        leftHandModel.SetActive(true);
        rightHandModel.SetActive(true);
        
        Debug.Log("Intro finished. Waiting for player to open the door...");
    }


    public void UnlockFinalMovement()
    {
        LockMovement(false);
        Debug.Log("Door opened! Player can now move freely.");
    }

    private void LockMovement(bool lockState)
    {
        if(moveProvider != null) moveProvider.enabled = !lockState;
        if(turnProvider != null) turnProvider.enabled = !lockState;
    }

    private IEnumerator FadeUI(Image img, float targetAlpha)
    {
       
        Color c = img.color;
        while (Mathf.Abs(c.a - targetAlpha) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            img.color = c;
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