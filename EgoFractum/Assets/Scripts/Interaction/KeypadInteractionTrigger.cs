using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KeypadInteractionTrigger : MonoBehaviour
{
    
    [SerializeField] GameObject[] nearFarInteractors;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            foreach (var nearFarInteractor in nearFarInteractors)
                nearFarInteractor.SetActive(false);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            foreach (var nearFarInteractor in nearFarInteractors)
                nearFarInteractor.SetActive(true);
    }
}
