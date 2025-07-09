using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        if (startButton == null)
            startButton = GetComponent<Button>();

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => LoadHintScene());
    }

    void LoadHintScene()
    {
        Debug.Log("Loading Hint Scene...");
        SceneManager.LoadScene("HintScene"); // Load the HintScene
    }

    // Add a method to load the first scene (scene index 0)
    public void LoadFirstScene()
    {
        Debug.Log("Loading First Scene...");
        SceneManager.LoadScene(4); // Load the first scene by index
    }
}
