using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

public class GamePause : MonoBehaviour
{
    public string pauseSceneName = "PauseScene"; // Name of the pause scene (Change to your actual scene name)
    public string mainGameScene = "MainGame";    // The main game scene to return to after pause

    private bool isPaused = false;

    void Start()
    {
        // Ensure the game starts unpaused
        Time.timeScale = 1f;
    }

    // Pause the game and load the pause scene
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Stop the game’s time flow
        // Load the pause scene (you can customize this scene to show the pause menu, etc.)
        SceneManager.LoadScene(pauseSceneName);
    }

    // Resume the game and return to the main game scene
    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game’s time flow
        // Load the main game scene to continue the game from where it left off
        SceneManager.LoadScene(7);
    }

    // Update is called once per frame
    void Update()
    {
        // Press Escape to pause the game and go to the pause scene
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();
        }
        // Press Escape again to resume the game and return to the main game scene
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ResumeGame();
        }
    }
}
