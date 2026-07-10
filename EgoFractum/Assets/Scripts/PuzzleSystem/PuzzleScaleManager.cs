using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleScaleManager : Puzzle
    {
        [SerializeField] private DoorController[] doorsToUnlock;
        [SerializeField] private ScalePlate leftScalePlate,  rightScalePlate;
        [SerializeField] private float leftScaleMultiplier, rightScaleMultiplier;
        [SerializeField] private TMPro.TMP_Text diffText;
    
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
            var leftMass = leftScalePlate.GetCurrentMass();
            var rightMass = rightScalePlate.GetCurrentMass();
            if (Mathf.Approximately(leftMass, 0) || Mathf.Approximately(rightMass, 0))
            {
                diffText.text = "0";
            }
            else
            {
                int diff = Mathf.RoundToInt(leftMass - rightMass);
                Debug.Log("Diferença dos pesos: " + diff);
                // vai buscar o numero ao canvas e liga a set filha do numero
                diffText.text = diff.ToString();
            }
           
        }
    }
}
