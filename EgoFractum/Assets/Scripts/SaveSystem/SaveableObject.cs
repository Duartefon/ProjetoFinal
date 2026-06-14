using UnityEngine;
using System.Collections.Generic;

public class SaveableObject : MonoBehaviour
{
    [Tooltip("The permanent unique ID for this specific object in the scene.")]
    public string uniqueID;

    // todos os objetos que são saveable
    public static List<SaveableObject> AllSaveableObjects = new List<SaveableObject>();

    void Awake()
    {
        // Quando o jogo começa o objeto adiciona-se a lista
        if (!AllSaveableObjects.Contains(this))
        {
            AllSaveableObjects.Add(this);
        }
    }

    void OnDestroy()
    {
        // so para o caso de trocas de scene
        if (AllSaveableObjects.Contains(this))
        {
            AllSaveableObjects.Remove(this);
        }
    }
}