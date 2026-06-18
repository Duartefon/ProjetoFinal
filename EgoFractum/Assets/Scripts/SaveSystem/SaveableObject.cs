using UnityEngine;
using System.Collections.Generic;

public class SaveableObject : MonoBehaviour
{
    [Tooltip("ID permanente do objeto")]
    public string uniqueID;

    // todos os objetos que são saveable
    public static HashSet<SaveableObject> AllSaveableObjects = new HashSet<SaveableObject>();

    void Awake()
    {
        AllSaveableObjects.Add(this);
    }

    void OnDestroy()
    {
        AllSaveableObjects.Remove(this);
    }
}