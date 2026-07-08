using System.Text.RegularExpressions;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] private string keyCode = "";
    [SerializeField] private string puzzleKey = "Weights";
    [SerializeField] private TMPro.TMP_Text screenText;
    [SerializeField] PuzzleScaleManager _puzzleScaleManager;
    
    private string _inputKeyCode = string.Empty;
    private bool _puzzleFinished = false;
    

    void Start()
    {
       
    }

    public void OnKeyPressed(string value)
    {
        Debug.Log($"KeyPressed: {value}");
        if(_inputKeyCode.Length < keyCode.Length)
            _inputKeyCode += value;

        SetText(_inputKeyCode);
    }

    public void OnEnterPressed()
    {
        VerifyKeyCode();
    }

    public void OnClearPressed()
    {
        //get string from start to last position-1, therefore excluding last char
        
        if(_inputKeyCode.Length > 0)
            _inputKeyCode = _inputKeyCode.Substring(0, _inputKeyCode.Length - 1);
        
        SetText(_inputKeyCode);
    }

    private void VerifyKeyCode()
    {
        if (_inputKeyCode.Equals(keyCode))
        {
            _puzzleFinished = true;
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            SetText("Correct!");
            _puzzleScaleManager.OnCompletePuzzle();
            
        }
        else
        {
            _inputKeyCode = "";
            SetText("");
        }
    }

    private void SetText(string text)
    {
        screenText.text = text.Length <= 0 ? "****" : text;
        }
}