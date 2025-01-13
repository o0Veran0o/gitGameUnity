using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<GameObject> itemPrefabs; // Assign item prefabs in the Inspector
    [SerializeField] private PlayerStats playerStats; // Reference to the PlayerStats component
    [SerializeField] private InventoryManager inventoryManager; // Reference to InventoryManager
    public TMP_Text timerText; // Assign in the Inspector
    private float startTime;
    private TimeSpan elapsedTime;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Ensure only one GameManager exists
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        // Update the timer
        elapsedTime = TimeSpan.FromSeconds(Time.time - startTime);
        timerText.text = string.Format("{0:00}:{1:00}", elapsedTime.Minutes, elapsedTime.Seconds);


    }
    private void Start()
    {
        // Initialize the item database with the item prefabs
        ItemDatabase.Initialize(itemPrefabs);

        // Ensure PlayerStats and InventoryManager are properly assigned
        if (playerStats == null || inventoryManager == null)
        {
            Debug.LogError("PlayerStats or InventoryManager not assigned in GameManager.");
            return;
        }
        CheckWorldLoadedState();
    }
    private void CheckWorldLoadedState()
    {
        int worldLoaded = PlayerPrefs.GetInt("WorldLoaded", 0); // Default to 0 if the key doesn't exist

        if (worldLoaded == 0)
        {
            // New game logic: World is not loaded (start a fresh world)
            Debug.Log("Starting a new game...");
       
        }
        else if (worldLoaded == 1)
        {
            // Loaded game logic: World is loaded from save
            Debug.Log("Loading saved game...");
            LoadSavedGame();
        }
        else
        {
            Debug.LogWarning("Unknown world state, starting a new game.");
      
        }
    }

    public void SaveGame()
    {
        // Call the save method from PlayerStats
        GameData data = new GameData
        {
            health = playerStats.GetHealth(),
            sanity = playerStats.GetSanity(),
            position = playerStats.transform.position,

            inventoryItems = inventoryManager.GetInventoryItems()
        };

        string saveFileName = "Save1.json"; // Adjust the file name as needed
        SaveManager.SaveGame(data, saveFileName);

        // Mark the world as loaded
        PlayerPrefs.SetInt("WorldLoaded", 1);
        PlayerPrefs.SetFloat("ElapsedTime", (float)elapsedTime.TotalSeconds);
        PlayerPrefs.Save();

        Debug.Log("Game saved.");
    }

    public void LoadSavedGame()
    {
        string saveFileName = "Save1.json"; // Adjust the file name as needed
        GameData data = SaveManager.LoadGame(saveFileName);

        if (data != null)
        {
            // Load player stats
            playerStats.LoadPlayerStats(data);
            startTime = Time.time - PlayerPrefs.GetFloat("ElapsedTime", 0f); // Default to 0 if not found

            // Load inventory
            inventoryManager.LoadInventoryItems(data.inventoryItems);

            // Set world position and other relevant data
            playerStats.transform.position = data.position;

            Debug.Log("Saved game loaded.");
        }
        else
        {
            Debug.LogError("Failed to load saved game.");
        }
    }
}
