using System;
using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; } // Singleton

    [Tooltip("Os puzzles são indexados por um 'key' que corresponde ao nome do puzzle.")]
    private Dictionary<string, bool> _puzzles = new Dictionary<string, bool>();

    public static event Action OnPuzzleCompleted;

   
    [SerializeField] protected string puzzleName;

    [SerializeField] protected bool puzzleFinished = false;

    //TODO: Remove serializefield after debugging
    [SerializeField] protected bool puzzleStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Dictionary<string, bool> GetPuzzles()
    {
        return _puzzles;
    }

    public void SetPuzzles(Dictionary<string, bool> puzzles)
    {
        this._puzzles = puzzles;
    }

    public void AddPuzzle(string key, bool completed)
    {
        _puzzles.Add(key, completed);
    }

    public void CompletePuzzle(string key)
    {
        _puzzles[key] = true;
        OnPuzzleCompleted?.Invoke();
    }

    public bool IsPuzzleCompleted(string key)
    {
        return _puzzles.ContainsKey(key) && _puzzles[key];
    }
}