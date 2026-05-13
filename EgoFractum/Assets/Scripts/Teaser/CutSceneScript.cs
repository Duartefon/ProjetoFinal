using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneScript : MonoBehaviour
{
    public Transform waypoint1, waypoint2;
    public Camera mainCamera;

    public float smoothTime = 1f;

    public bool playCutscene = false;
    public bool triggerFlicker = false;
    public bool everythingOn = true;

    public Image eyeIcon;
    public Light[] pcLights;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.transform.position = waypoint1.position;
    }

    void Update()
    {
        // Replay cutscene from Inspector
        if (playCutscene)
        {
            PlayCutscene();
            playCutscene = false;
        }

        // Trigger flicker from Inspector
        if (triggerFlicker)
        {
            FlickerAndOff();
            triggerFlicker = false;
        }

        // Turn everything on from Inspector
        if (everythingOn && !eyeIcon.enabled)
        {
            eyeIcon.enabled = true;
            foreach (Light light in pcLights)
            {
                light.enabled = true;
            }
            everythingOn = false;
        }

        // Camera movement
        if (velocity != Vector3.zero)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(
                mainCamera.transform.position,
                waypoint2.position,
                ref velocity,
                smoothTime
            );

            if (Vector3.Distance(mainCamera.transform.position, waypoint2.position) < 0.01f)
            {
                mainCamera.transform.position = waypoint2.position;
                velocity = Vector3.zero;
            }
        }
    }

    public void PlayCutscene()
    {
        mainCamera.transform.position = waypoint1.position;

        velocity = Vector3.zero;

        velocity = Vector3.one * 0.01f;
    }

    public void FlickerAndOff()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        for (int i = 0; i < 6; i++)
        {
            // Toggle eye icon
            eyeIcon.enabled = !eyeIcon.enabled;

            // Toggle all lights
            foreach (Light light in pcLights)
            {
                light.enabled = !light.enabled;
            }

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        // Final OFF state
        eyeIcon.enabled = false;

        foreach (Light light in pcLights)
        {
            light.enabled = false;
        }
    }
}