using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;

public class SaveSystemEditor
{
    /**
     * Este script permite que, a partir do menu "Tools" do Unity, seja
     * adicionado automaticamente o script SaveableObject a todos os objetos
     * interativos (com Rigidbody ou Interactable), atribuindo-lhes um ID
     * aleatório que os identifica.
     */
    [MenuItem("Tools/Setup Saveable Objects")]
    public static void SetupAllXRObjects()
    {
        var allGrabbables = Object.FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.InstanceID);
        var allRigidbodies = Object.FindObjectsByType<Rigidbody>(FindObjectsSortMode.InstanceID);
        HashSet<GameObject> objectsToSetup = new HashSet<GameObject>();

        foreach (var grabbable in allGrabbables)
        {
            objectsToSetup.Add(grabbable.gameObject);
        }

        foreach (var rb in allRigidbodies)
        {
            if (!rb.isKinematic && !rb.gameObject.CompareTag("Player"))
            {
                objectsToSetup.Add(rb.gameObject);
            }
        }

        int addedScripts = 0;
        int addedIDs = 0;

        foreach (GameObject obj in objectsToSetup)
        {
            SaveableObject saveScript = obj.GetComponent<SaveableObject>();
            if (saveScript == null)
            {
                saveScript = obj.AddComponent<SaveableObject>();
                addedScripts++;
            }

            if (string.IsNullOrEmpty(saveScript.uniqueID))
            {
                Undo.RecordObject(saveScript, "Generated Unique ID"); 
                saveScript.uniqueID = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(saveScript); 
                addedIDs++;
            }
        }

        Debug.Log($"Setup de objetos completo. Adicionados {addedScripts} scripts e gerados {addedIDs} IDs em {objectsToSetup.Count} objetos totais.");
    }
}