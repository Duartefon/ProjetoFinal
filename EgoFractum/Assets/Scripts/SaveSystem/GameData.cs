using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [System.Serializable]
    public class ObjectTransformData
    {
        public string id;
        public Vector3 position;
        public Quaternion rotation;
    }

    [System.Serializable]
    public class PlayerData
    {
        [Header("Player Position")]
        public float[] transform;

        [Header("Completed Puzzles")]
        [Tooltip("Os puzzles são indexados por um 'key' que corresponde ao nome do puzzle.")]
        public List<string> puzzleKeys = new();
        public List<bool> puzzleValues = new();
        public List<ObjectTransformData> movableObjects = new();
    }
}