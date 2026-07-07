using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; } //Singleton

    [Header("Player Position")]
    public Transform playerTransform;

    [Header("UI Elements")]
    public TMP_Text saveText;

    [Header("Test")]
    public bool triggerSaveUI;
    public bool saveGame;
    [Tooltip("Ligar isto para testar o 'Load' ao começar o 'Play Mode'.")]
    public bool loadGameOnStart;

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

    private void Start()
    {
        if (loadGameOnStart)
        {
            LoadGame();
        }

        if (saveText != null)
        {
            saveText.gameObject.SetActive(false);
        }
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        

    }

    private void Update()
    {
        if (triggerSaveUI)
        {
            triggerSaveUI = false;
            if (saveText != null)
            {
                StopAllCoroutines();
                StartCoroutine(BlinkSaveTextRoutine());
            }
        }

        if (saveGame)
        {
            saveGame = false;
            SaveGame();
            Debug.Log("[TEST] Saving game...");
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

        if (saveText != null)
        {
            StopAllCoroutines();
            StartCoroutine(BlinkSaveTextRoutine());
        }
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

            foreach (var objData in data.movableObjects)
            {
                foreach (SaveableObject obj in SaveableObject.AllSaveableObjects)
                {
                    if (obj.uniqueID == objData.id)
                    {
                        obj.transform.position = objData.position;
                        obj.transform.rotation = objData.rotation;
                        break;
                    }
                }
            }

        }
    }

    private IEnumerator BlinkSaveTextRoutine()
    {
        saveText.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            Color color = saveText.color;
            color.a = 0f;
            saveText.color = color;

            yield return StartCoroutine(FadeAlpha(0f, 1f, 0.5f));

            yield return new WaitForSeconds(0.75f);

            yield return StartCoroutine(FadeAlpha(1f, 0f, 0.5f));

            yield return new WaitForSeconds(0.2f);
        }

        saveText.gameObject.SetActive(false);
    }

    private IEnumerator FadeAlpha(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = saveText.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            saveText.color = color;

            yield return null;
        }

        color.a = endAlpha;
        saveText.color = color;
    }

}
