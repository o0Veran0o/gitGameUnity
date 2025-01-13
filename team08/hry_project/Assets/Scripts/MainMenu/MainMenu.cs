using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        // Add listeners to the buttons
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(StartNewGame);
        }

        if (loadGameButton != null)
        {
            loadGameButton.onClick.AddListener(LoadGame);
        }
       
    }


    // Method to start a new game
    private void StartNewGame()
    {
        Debug.Log("Starting New Game...");

        // Set flag for new game
        PlayerPrefs.SetInt("WorldLoaded", 0); // 0 means new game (not loaded)
        PlayerPrefs.Save();

        // Load the game scene
        SceneManager.LoadScene("SampleScene");
    }

    // Method to load a saved game
    private void LoadGame()
    {
        Debug.Log("Loading Game...");

        // Set flag for loaded game
        PlayerPrefs.SetInt("WorldLoaded", 1); // 1 means game is loaded from a save
        PlayerPrefs.Save();

        // Load the game scene
        SceneManager.LoadScene("SampleScene");
    }
}
