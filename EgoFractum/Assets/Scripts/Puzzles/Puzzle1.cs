using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private string keyCode = "9347" ;
    private string _inputKeyCode = string.Empty;
    private bool _puzzleFinished = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKeyPressed(string key)
    {
        Debug.Log($"KeyPressed: {key}");
        _inputKeyCode += key;
        if (_inputKeyCode == keyCode)
            _puzzleFinished = true;
        else if (_inputKeyCode.Length >= 4)
        {
            Debug.Log("Wrong Keycode");
        }

    }
    public void OnEnterPressed()
    { 
        if (_inputKeyCode == keyCode)
            _puzzleFinished = true;
        else if (_inputKeyCode.Length >= 4)
        {
            Debug.Log("Wrong Keycode");
        }

    }
    
    public void OnClearPressed()
    { 
        //get string from start to last position-1, therefore excluding last char
        _inputKeyCode = _inputKeyCode.Substring(0, _inputKeyCode.Length - 1);

    }
}
