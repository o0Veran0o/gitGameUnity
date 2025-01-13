using UnityEngine;
using UnityEngine.UI; // For Button references
using UnityEngine.SceneManagement; // To reload scene or manage game states

public class GameMenu : MonoBehaviour
{
    // References to the buttons in the pause menu
    [SerializeField] private Button continueButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;

    // Reference to the GameManager and PlayerStats
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlayerHandler PlayerHandler;
    // Reference to the Pause Menu UI
    [SerializeField] private GameObject pauseMenuUI;

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        pauseMenuUI.SetActive(false);

        // Add listeners to the buttons
        continueButton.onClick.AddListener(ContinueGame);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Called when the ESC key is pressed to toggle the pause menu
    public void TogglePauseMenu()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Called when the Continue button is pressed
    public void ContinueGame()
    {
        ResumeGame(); // Just resume the game
    }

    // Called when the Save button is pressed
    public void SaveGame()
    {
        gameManager.SaveGame(); // Delegate to GameManager to save
        ResumeGame(); // Optionally resume game after saving
    }

    // Called when the Load button is pressed
    public void LoadGame()
    {
        gameManager.LoadSavedGame(); // Delegate to GameManager to load
        ResumeGame(); // Optionally resume game after loading
    }

    // Called when the Quit button is pressed
    public void QuitGame()
    {
        // Optionally save the game before quitting if needed
        // gameManager.SaveGame();
        ResumeGame();
        SceneManager.LoadScene("MenuScene"); // Change "MainMenu" to your main menu scene name
    }

    // Pause the game and show the menu
    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause game time
        pauseMenuUI.SetActive(true); // Show the pause menu UI
    }

    // Resume the game and hide the menu
    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game time
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        PlayerHandler.enabled = true;
        inventoryManager.enabled = true;
    }
}
