using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneScript : MonoBehaviour
{
    [SerializeField] private Transform waypoint1, waypoint2;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float smoothTime = 1f;

    [SerializeField] private bool playCutscene = false;
    [SerializeField] private bool triggerFlicker = false;
    [SerializeField] private bool everythingOn = true;

    [SerializeField] private Image eyeIcon;
    [SerializeField] private Light[] pcLights;

    private Vector3 _velocity = Vector3.zero;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.transform.position = waypoint1.position;
    }
    
    /**
     * Este script foi apenas utilizado para gravar o "teaser" usado na apresentação
     * no FEIM 2026. Fora isso, não tem uso prático durante o jogo.
     */

    void Update()
    {
        if (playCutscene)
        {
            PlayCutscene();
            playCutscene = false;
        }
        
        if (triggerFlicker)
        {
            FlickerAndOff();
            triggerFlicker = false;
        }

        if (everythingOn && !eyeIcon.enabled)
        {
            eyeIcon.enabled = true;
            foreach (Light pcLight in pcLights)
            {
                pcLight.enabled = true;
            }

            everythingOn = false;
        }

        if (_velocity != Vector3.zero)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(
                mainCamera.transform.position,
                waypoint2.position,
                ref _velocity,
                smoothTime
            );

            if (Vector3.Distance(mainCamera.transform.position, waypoint2.position) < 0.01f)
            {
                mainCamera.transform.position = waypoint2.position;
                _velocity = Vector3.zero;
            }
        }
    }

    private void PlayCutscene()
    {
        mainCamera.transform.position = waypoint1.position;

        _velocity = Vector3.zero;

        _velocity = Vector3.one * 0.01f;
    }

    private void FlickerAndOff()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        for (int i = 0; i < 6; i++)
        {
            eyeIcon.enabled = !eyeIcon.enabled;

            foreach (Light pcLight in pcLights)
            {
                pcLight.enabled = !pcLight.enabled;
            }

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        eyeIcon.enabled = false;

        foreach (Light pcLight in pcLights)
        {
            pcLight.enabled = false;
        }
    }
}