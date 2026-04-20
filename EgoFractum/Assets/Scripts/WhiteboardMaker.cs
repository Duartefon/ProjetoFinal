using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class WhiteboardMaker : MonoBehaviour
{

    public Transform _markerTip;

    
    public int _penSize = 5;

    private Renderer _renderer;

    private Color[] _colors;

    public  float tipHeight = 3f;
    private WhiteBoard _whiteBoard;

    private Vector2 _touchPos, _lastTouchPos;
    private     RaycastHit _hit;
    private bool _touchedLastFrame;

    public float interpolationStep = 0.01f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = _markerTip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        tipHeight = _markerTip.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
         Debug.DrawRay (_markerTip.position, transform.forward* tipHeight, Color.brown);
        if (Physics.Raycast(_markerTip.position, transform.forward, out _hit, tipHeight))
        {
            if (!_hit.transform.CompareTag("Whiteboard"))
            {
                _whiteBoard = null;
                _touchedLastFrame = false;
                return;
            }
            
            Debug.Log($"Hit: {_hit}");

            if (!_whiteBoard)
                _whiteBoard = _hit.transform.GetComponent<WhiteBoard>();
            
            _touchPos = new Vector2(_hit.textureCoord.x, _hit.textureCoord.y);

            var x = (int)(_touchPos.x * _whiteBoard.textureSize.x - _penSize / 2); //convert x and y to pixelX and pixelY of the whiteboard texture

            var y = (int)(_touchPos.y * _whiteBoard.textureSize.y - _penSize / 2);

            if (y < 0 || y > _whiteBoard.textureSize.y || x < 0 || x > _whiteBoard.textureSize.x) return;

            if (_touchedLastFrame)
            {
                _whiteBoard.texture.SetPixels(x,y, _penSize, _penSize, _colors);
                for (float f = 0.01f; f < 1.00f; f += interpolationStep ) //f steps into 100% to interpolate between current position and next position. TODO: test f += DeltaTime
                {
                    var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                    var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);

                    _whiteBoard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                } 
                
                _whiteBoard.texture.Apply();
            }

            _lastTouchPos = new Vector2(x, y);
            _touchedLastFrame = true;
        }
    }
}
