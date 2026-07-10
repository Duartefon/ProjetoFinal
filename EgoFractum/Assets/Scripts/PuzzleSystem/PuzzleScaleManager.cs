using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleScaleManager : Puzzle
    {
        [SerializeField] private DoorController[] doorsToUnlock;
        [SerializeField] private ScalePlate leftScalePlate,  rightScalePlate;
        [SerializeField] private float leftScaleMultiplier, rightScaleMultiplier;
    
        public void OnCompletePuzzle()
        {
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            
            foreach (var door in doorsToUnlock)
            {
                door.OpenWithoutGenerator();
            }
        }

        void Update()
        {
            CompareWeights();
        }

        private void CompareWeights()
        {
            // compara o peso de cada prato e faz floor (0 - 4)
            int diff = Mathf.FloorToInt(leftScalePlate.GetCurrentMass() -  rightScalePlate.GetCurrentMass());
            Debug.Log("Diferença dos pesos: " + diff);
            // vai buscar o numero ao canvas e liga a set filha do numero
            
        }
    }
}
