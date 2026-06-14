using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; } //Singleton

    [Header("Player Position")]
    public Transform playerTransform;

    // O save poderá ser periódico ou apenas ao fim de cada puzzle.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveGame()
    {
        Debug.Log("Saving game...");
        PlayerData data = new PlayerData();
        data.transform = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z };

        Dictionary<string, bool> currentPuzzles = PuzzleManager.Instance.GetPuzzles();
        foreach (var kvp in currentPuzzles)
        {
            data.puzzleKeys.Add(kvp.Key);
            data.puzzleValues.Add(kvp.Value);
        }

        foreach (SaveableObject obj in SaveableObject.AllSaveableObjects)
        {
            if (string.IsNullOrEmpty(obj.uniqueID)) continue;

            data.movableObjects.Add(new ObjectTransformData
            {
                id = obj.uniqueID,
                position = obj.transform.position,
                rotation = obj.transform.rotation
            });
        }
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(path, json);
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
        string path = Application.persistentDataPath + "/playerData.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            Dictionary<string, bool> loadedPuzzles = new Dictionary<string, bool>();
            for (int i = 0; i < data.puzzleKeys.Count; i++)
            {
                loadedPuzzles.Add(data.puzzleKeys[i], data.puzzleValues[i]);
            }

            playerTransform.position = new Vector3(data.transform[0], data.transform[1], data.transform[2]);
            PuzzleManager.Instance.SetPuzzles(loadedPuzzles);


        }
    }

}
