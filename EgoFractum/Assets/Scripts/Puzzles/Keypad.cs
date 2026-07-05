using System.CodeDom.Compiler;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] private string keyCode = "";
    [SerializeField] private string puzzleKey = "Weights";
    [SerializeField] private TMPro.TMP_Text screenText;
    private string _inputKeyCode = string.Empty;
    private bool _puzzleFinished = false;

    void Start()
    {
        SetText("****");
    }

    public void OnKeyPressed(string value)
    {
        Debug.Log($"KeyPressed: {value}");

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
        _inputKeyCode = _inputKeyCode.Substring(0, _inputKeyCode.Length - 1);
        SetText(_inputKeyCode);
    }

    private void VerifyKeyCode()
    {
        if (_inputKeyCode.Equals(keyCode))
        {
            _puzzleFinished = true;
            PuzzleManager.Instance.CompletePuzzle(puzzleKey);
            Debug.Log("Puzzle finished!");
        }
        else
        {
            _inputKeyCode = "";
        }
    }

    private void SetText(string text)
    {
        screenText.text = text;
    }
}