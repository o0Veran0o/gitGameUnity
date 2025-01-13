using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;  // For Light2D
using UnityEngine.SceneManagement;     // For scene reloading

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxSanity = 100f;

    private float currentHealth;
    private float currentSanity;

    private bool isFlashLightOn = true;
    private bool isHealthDraining = false;

    private InventoryManager inventoryManager;
 
    // Reference to the player's 2D light
    [SerializeField] private Light2D playerLight;  // Make sure to assign this in the Inspector

    // Reference to the Pause Menu UI
    [SerializeField] private GameObject pauseMenuUI;

    // Flag to check if the game is paused
    private bool isPaused = false;
    private PlayerHandler PlayerHandle;
    void Start()
    {
        currentHealth = maxHealth / 2;
        currentSanity = maxSanity / 2;
        inventoryManager = GetComponent<InventoryManager>();
        PlayerHandle = GetComponent<PlayerHandler>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found! Ensure it is attached to the player.");
        }
        StartCoroutine(SanityDrainOverTime());

        // Ensure that the pause menu is not visible at the start
       // pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Check for ESC key press to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        CheckPlayerDeath();

        // If sanity is less than 50%, start draining health
        if (currentSanity < maxSanity * 0.5f && !isHealthDraining)
        {
            StartCoroutine(HealthDrainOverTime());
        }


    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true); // Show the pause menu
        inventoryManager.enabled = false;
        PlayerHandle.enabled = false;
        Debug.Log("Game paused");
        Time.timeScale = 0f; // Pause the game
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false); // Hide the pause menu
        PlayerHandle.enabled = true;
        inventoryManager.enabled = true;
        Time.timeScale = 1f; // Resume the game
        Debug.Log("Game unpaused");
    }


    private void CheckPlayerDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player has died.");
            PauseGame();
            // Add death handling logic here
        }
    }

    private IEnumerator HealthDrainOverTime()
    {
        isHealthDraining = true;
        while (currentSanity < maxSanity * 0.5f && currentHealth > 0)
        {
            currentHealth -= 1f; // Lose 1 HP every second
            Debug.Log("Health draining: " + currentHealth);
  
            yield return new WaitForSeconds(1f);
        }
        isHealthDraining = false;
    }

    private IEnumerator SanityDrainOverTime()
    {
        while (!isFlashLightOn)
        {
            currentSanity -= 1f; // Lose 1 sanity per second
            Debug.Log("Sanity draining: " + currentSanity);
     
            yield return new WaitForSeconds(1f);
        }
    }

    public void Heal(float hp)
    {
        currentHealth += hp;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void HealSanity(float sp)
    {
        currentSanity += sp;
        if (currentSanity > maxSanity)
            currentSanity = maxSanity;
    }



    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetSanity()
    {
        return currentSanity;
    }

    public bool ToggleFlashlight()
    {
        isFlashLightOn = !isFlashLightOn;

        return isFlashLightOn;
    }
    public void TakeDmg(int dmg) { 
        this.currentHealth-= dmg;
    }

    public void LoadPlayerStats(GameData data)
    {
        currentHealth = data.health;
        currentSanity = data.sanity;
    }
}
