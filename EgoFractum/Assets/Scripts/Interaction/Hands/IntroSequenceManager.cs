using System;
using System.Collections;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Drives the opening and closing cinematics. The intro shows a console quote and hands
/// control back to the player; the ending plays out and rolls the credits.
/// </summary>
public class IntroSequenceManager : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private Image blackScreen;

    [SerializeField] private TextMeshProUGUI introConsoleText;
    [SerializeField] private TextMeshProUGUI endingConsoleText;
    [SerializeField] private TextMeshProUGUI introHudText;
    [SerializeField] private TextMeshProUGUI mazeHudText;

    [Header("Audio")] [SerializeField] private AudioSource introAudio;

    [Header("Player")] [Tooltip("Locomotion root. Disabled while a sequence plays.")] [SerializeField]
    private GameObject locomotion;

    [Tooltip("Leave empty to resolve by the 'Player' tag at runtime.")] [SerializeField]
    private Transform player;

    [SerializeField] private GameObject[] leftHandModel;
    [SerializeField] private GameObject[] rightHandModel;

    [Header("Transfer Points")] [SerializeField]
    private PlayerTransferData introStartPosition;

    [SerializeField] private PlayerTransferData introEndPosition;
    [SerializeField] private PlayerTransferData endingStartPosition;
    [SerializeField] private TransitionEffectManager transitionEffectManager;

    [Header("Intro Timings (seconds)")] [Tooltip("Silence before the intro quote appears.")] [SerializeField, Min(0f)]
    private float delayBeforeQuote = 3f;

    [Tooltip("How long the intro quote stays on screen.")] [SerializeField, Min(0f)]
    private float quoteStayTime = 6.5f;

    [Tooltip("How long the HUD hint stays up after the player arrives.")] [SerializeField, Min(0f)]
    private float hudHintDuration = 6.5f;

    [Header("Credits (ending only)")]
    [Tooltip("Pause after the transition effect before the credits fade in.")]
    [SerializeField, Min(0f)]
    private float delayBeforeCredits = 1f;

    [Tooltip("How long the credits stay fully visible.")] [SerializeField, Min(0f)]
    private float creditsHoldTime = 8f;

    [Tooltip("Alpha change per second. Only used by the credits fade.")] [SerializeField, Min(0.01f)]
    private float fadeSpeed = 1f;

    [Tooltip("Fade the black screen in behind the credits.")] [SerializeField]
    private bool fadeToBlackForCredits = true;

    [Tooltip("How long the ending quote stays on screen.")] [SerializeField, Min(0f)]
    private float endingQuoteStayTime = 6.5f;

    [Header("Credits (ending only)")]
    [Tooltip("The credits roll. Can be the same object as endingConsoleText if you reuse it.")]
    [SerializeField]
    private GameObject endingPanel;

    [SerializeField] private TextMeshProUGUI thankyouText;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private Image logo;
    [SerializeField] private TextMeshProUGUI specialThanksText;
    private Coroutine _sequence;
    private bool _hasIntroPlayed = false;

    /// <summary>True while an intro or ending sequence is playing.</summary>
    public bool IsPlaying => _sequence != null;

    private void Awake()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
            else Debug.LogError($"{nameof(IntroSequenceManager)}: no GameObject tagged 'Player' found.", this);
        }

        SetTextVisible(introConsoleText, false);
        SetTextVisible(introHudText, false);
        SetTextVisible(endingConsoleText, false, 0f);
        SetGraphicAlpha(blackScreen, 0f);
    }



    public void Start()
    {
        if (_hasIntroPlayed) return;
        StartIntro();
        _hasIntroPlayed = true;
    }

    public void StartIntro()
    {
        if (!CanStart(introStartPosition)) return;
        _sequence = StartCoroutine(PlayIntroSequence());
    }

    public void StartEnding()
    {
        if (!CanStart(endingStartPosition)) return;
        _sequence = StartCoroutine(PlayEndSequence());
    }

    private bool CanStart(PlayerTransferData startPoint)
    {
        if (_sequence != null)
        {
            Debug.LogWarning($"{nameof(IntroSequenceManager)}: sequence already playing, ignoring request.", this);
            return false;
        }

        if (startPoint == null)
        {
            Debug.LogError($"{nameof(IntroSequenceManager)}: start point not assigned.", this);
            return false;
        }

        return true;
    }

    private IEnumerator PlayIntroSequence()
    {
        transitionEffectManager.TransitionPlayerTo(player, introStartPosition);

        LockMovement(true);
        SetHandsVisible(false);
        introAudio.Play();

        yield return new WaitForSeconds(delayBeforeQuote);

        // Snap on/off — no fade on the intro quote.
        SetTextVisible(introConsoleText, true, 1f);
        yield return new WaitForSeconds(quoteStayTime);
        SetTextVisible(introConsoleText, false);

        transitionEffectManager.PlayEffect();
        yield return new WaitForSeconds(transitionEffectManager.effectTime);

        transitionEffectManager.TransitionPlayerTo(player, introEndPosition);

        SetHandsVisible(true);
        introAudio.Stop();

        SetTextVisible(introHudText, true);
        yield return new WaitForSeconds(hudHintDuration);
        SetTextVisible(introHudText, false);

        LockMovement(false);
        _sequence = null;

        Debug.Log("Intro finished. Waiting for player to open the door...");
    }
    public void OnPlayMazeSequence()
    {
        StartCoroutine(MazeSequence());
    }
    private IEnumerator MazeSequence()
    {
        SetTextVisible(mazeHudText, true);
        yield return new WaitForSeconds(hudHintDuration);
        SetTextVisible(mazeHudText, false);
    }

    private IEnumerator PlayEndSequence()
    {
        transitionEffectManager.TransitionPlayerTo(player, endingStartPosition);

        LockMovement(true);
        SetHandsVisible(false);
        introAudio.Play();

        yield return new WaitForSeconds(delayBeforeQuote);

        // Same as the intro: snap the quote on, hold, snap it off.
        SetTextVisible(endingConsoleText, true, 1f);
        yield return new WaitForSeconds(endingQuoteStayTime);
        SetTextVisible(endingConsoleText, false);

        transitionEffectManager.PlayEffect();
        yield return new WaitForSeconds(transitionEffectManager.effectTime);

        introAudio.Stop();

        // Now fade to black, then roll the credits on top of it.
        yield return FadeGraphic(blackScreen, 1f);
        yield return new WaitForSeconds(delayBeforeCredits);

        endingPanel.SetActive(true);

        yield return FadeGraphic(thankyouText, 1f);
        yield return FadeGraphic(logo, 1f);
        yield return new WaitForSeconds(creditsHoldTime);
        yield return FadeGraphic(thankyouText, 0f);
        yield return FadeGraphic(logo, 0f);


        yield return FadeGraphic(endingText, 1f);
        yield return new WaitForSeconds(creditsHoldTime);
        yield return FadeGraphic(endingText, 0f);

        yield return FadeGraphic(specialThanksText, 1f);
        yield return new WaitForSeconds(creditsHoldTime);
        yield return FadeGraphic(specialThanksText, 0f);
        yield return new WaitForSeconds(creditsHoldTime);

        _sequence = null;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Ending finished. Credits complete.");
    }

    public void UnlockFinalMovement()
    {
        LockMovement(false);
        Debug.Log("Door opened! Player can now move freely.");
    }

    private void LockMovement(bool locked) => locomotion.SetActive(!locked);

    private void SetHandsVisible(bool visible)
    {
        SetActiveAll(leftHandModel, visible);
        SetActiveAll(rightHandModel, visible);
    }

    private static void SetActiveAll(GameObject[] objects, bool active)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj != null) obj.SetActive(active);
        }
    }

    private static void SetTextVisible(TextMeshProUGUI text, bool enabled, float? alpha = null)
    {
        if (text == null) return;

        if (alpha.HasValue) SetGraphicAlpha(text, alpha.Value);
        text.enabled = enabled;
    }

    private static void SetGraphicAlpha(Graphic graphic, float alpha)
    {
        if (graphic == null) return;

        Color c = graphic.color;
        c.a = Mathf.Clamp01(alpha);
        graphic.color = c;
    }

    /// <summary>
    /// Fades any Graphic (Image, TextMeshProUGUI, ...) to a target alpha at <see cref="fadeSpeed"/> per second.
    /// Converges exactly on the target, so 0f and 1f are both safe end values.
    /// </summary>
    private IEnumerator FadeGraphic(Graphic graphic, float targetAlpha)
    {
        if (graphic == null) yield break;

        targetAlpha = Mathf.Clamp01(targetAlpha);
        Color color = graphic.color;

        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            graphic.color = color;
            yield return null;
        }
    }
}