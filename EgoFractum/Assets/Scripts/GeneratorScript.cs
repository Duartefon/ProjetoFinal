using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    private int fuseCount = 2;
    public bool isOn = false;
    private AudioSource audioSource;
    private GameObject fuseTransform;

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn)
        {
            audioSource.Play();
        }
    }

    public void AddFuse()
    {
        // idk
        // gerador fica pronto para a cena da voltagem
    }

    public void SetVoltage(float voltage)
    {
        // 600V
    }
}
