using Gameplay;
using UnityEngine;

public class PuzzleLight : MonoBehaviour
{
    [Header("Puzzle Light")]
    [SerializeField] private Light puzzleLight;

    void OnEnable()
    {
        GeneratorScript.OnEnergyEstablished += TurnGreen;
        GeneratorScript.OnEnergyLost += TurnRed;
    }

    void OnDisable()
    {
        GeneratorScript.OnEnergyEstablished -= TurnGreen;
        GeneratorScript.OnEnergyLost -= TurnRed;
    }
    
    private void TurnGreen()
    {
        puzzleLight.color = new Color(0.3f, 0.8f, 0f, 1f);
    }

    private void TurnRed()
    {
        puzzleLight.color = new Color(1f, 0f, 0f, 1f);
    }
}
