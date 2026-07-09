using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; } // Singleton

    [Tooltip("Os puzzles são indexados por um 'key' que corresponde ao nome do puzzle.")]
    public Dictionary<string, bool> puzzles = new Dictionary<string, bool>();

    [SerializeField] protected string puzzleName;

    [SerializeField] protected bool puzzleFinished = false;

    //TODO: Remove serializefield after debugging
    [SerializeField] protected bool puzzleStarted = false;


    private void Awake()
    {

        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        AddPuzzle(puzzleName, puzzleFinished);
    }

    public Dictionary<string, bool> GetPuzzles()
    {
        return puzzles;
    }

    public void SetPuzzles(Dictionary<string, bool> puzzles)
    {
        this.puzzles = puzzles;
    }

    public void AddPuzzle(string key, bool completed)
    {
        puzzles.Add(key, completed);
    }
        //TODO: provavelmente sera protected
    public void CompletePuzzle(string key)
    {
        puzzles[key] = true;

        // talvez dar save aqui
        if (GameDataManager.Instance != null)
        {
           
            GameDataManager.Instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("[PUZZLE-MANAGER] GameDataManager is missing.");
        }
    }

    public bool IsPuzzleCompleted(string key)
    {
        return puzzles.ContainsKey(key) && puzzles[key];
    }
}