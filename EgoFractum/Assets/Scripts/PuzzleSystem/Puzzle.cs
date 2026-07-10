using UnityEngine;

namespace PuzzleSystem
{
    public class Puzzle : MonoBehaviour
    {
        [SerializeField] protected string puzzleKey;
        [SerializeField] protected bool puzzleStarted;
        [SerializeField] protected bool puzzleSolved;
    }
}
