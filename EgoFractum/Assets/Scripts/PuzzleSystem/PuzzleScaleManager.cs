using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleScaleManager : Puzzle
    {
        [SerializeField] private ScalePlate leftScalePlate, rightScalePlate;
        [SerializeField] private float leftScaleMultiplier, rightScaleMultiplier;
        [SerializeField] private TMPro.TMP_Text diffText;

        public void OnCompletePuzzle()
        {
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            UnlockDoors();
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
                diffText.text = "L - R = 0";
            }
            else
            {
                int diff = Mathf.RoundToInt(leftMass - rightMass);

                // vai buscar o numero ao canvas e liga a set filha do numero
                diffText.text = "L - R = " + diff;
            }
        }
    }
}