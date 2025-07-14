using UnityEngine;
using UnityEngine.UI; // For interacting with UI elements

public class ResumeGame : MonoBehaviour
{
    public GameObject pauseMenu;   // The Pause menu panel
    public Button resumeButton;    // The button to resume the game
    public Button quitButton;      // The button to quit the game

    private bool isPaused = false;

    void Start()
    {
        // Add listeners to buttons
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);

        // Make sure the pause menu is hidden at the start
        pauseMenu.SetActive(false);
    }

    // Function to resume the game
    void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes the game’s time flow
        pauseMenu.SetActive(false); // Hide the pause menu
    }

    // Function to quit the game
    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop the game in editor
        #else
            Application.Quit(); // Quit the game if built
        #endif
    }

    // Optional: Listen for Escape key to resume the game
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Resume(); // Resume the game when Escape is pressed while paused
        }
    }
}
