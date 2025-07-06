using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider; // Reference to the UI Slider for progress

    public GameObject player;
    public GameObject LoadCanvas;
    public List<GameObject> levels; 
    private int currentLevelIndex = 0;

    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Gem.onGemCollect += IncreasingProgressAmount;
        LoadCanvas.SetActive(false); // Hide the loading canvas at the start
    }
    
    void IncreasingProgressAmount(int amount)
    {
        progressAmount += amount;
       progressSlider.value = progressAmount; // Update the slider value
       if(progressAmount >= 100)
       {
            // Level Complete!
            Debug.Log("Level Complete!");
           // Here you can add logic to handle the win condition, like loading a new scene or showing a win UI
       }
    }

    void Update()
    {
        // Check if the player has collected enough gems to complete the level
        if (progressAmount >= 100)
        {
            LoadCanvas.SetActive(true); // Show the loading canvas when the level is complete
        }
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[nextLevelIndex].gameObject.SetActive(true);

        player.transform.position = new Vector3(0, 0, 0); // Reset player position

        currentLevelIndex = nextLevelIndex;
        progressAmount = 0; // Reset progress for the new level 
        progressSlider.value = 0; // Reset the slider value
    }
}
