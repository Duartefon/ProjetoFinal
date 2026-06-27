using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GeneratorScript : MonoBehaviour
{
    private int fuseCount = 2;
    public bool isOn = false, energyEstablished = false;
    private float voltage = 0;
    public AudioSource audioSource;
    public XRSocketInteractor leverSocket, fuseSocket;

    public string puzzleKey = "Generator";
    private bool isComplete = false;

    void Start()
    {
        //audioSource = GetComponentInChildren<AudioSource>();
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleKey))
        {
            isComplete = true;
            isOn = true;
            energyEstablished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // troquei para usar o event Select Entered
        /*
        if (fuseSocket.hasSelection)
        {
            AddFuse();
        }
        */

        if (energyEstablished && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (!isComplete && voltage == 600f && leverSocket.hasSelection)
        {
            Debug.Log("Everything is on!");
            energyEstablished = true;
            isComplete = true;

            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
        }
    }

    public void AddFuse()
    {
        isOn = true;
        fuseCount++;
    }

    public void RemoveFuse()
    {
        isOn = false;
        energyEstablished = false;
        fuseCount--;
    }

    public void SetVoltage(float voltage)
    {
        // 600V
        this.voltage = voltage;
    }


}
