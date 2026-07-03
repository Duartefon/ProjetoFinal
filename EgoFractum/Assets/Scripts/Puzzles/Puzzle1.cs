using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private string keyCode = "" ;
    [SerializeField] private TMPro.TMP_Text screenText;
    private string _inputKeyCode = string.Empty;
    private bool _puzzleFinished = false;
    void Start()
    {
        SetText( "CODIGO");
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKeyPressed(string key)
    {
        Debug.Log($"KeyPressed: {key}");
        
        _inputKeyCode += key;
        
        SetText( _inputKeyCode);
        
        
     
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
            
            Debug.Log("Puzzle finished!");
            SetText("Success!");
            
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
