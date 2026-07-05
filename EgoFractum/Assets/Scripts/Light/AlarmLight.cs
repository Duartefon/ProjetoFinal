using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    private bool _isActive = false;
    [SerializeField] float rotationAmount = 3f;

    public void Play()
    {
        _isActive = true;
        //GetComponent<AudioSource>().Play();
    }

    private void RotateLight()
    {
        if (!_isActive) return;
        transform.Rotate(0, 0, rotationAmount * Time.deltaTime, Space.World);
    }
    void Update()
    {
        RotateLight();
    }
}