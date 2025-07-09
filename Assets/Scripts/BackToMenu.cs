using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    public Button backButton;

    void Start()
    {
        if (backButton == null)
            backButton = GetComponent<Button>();

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => LoadMainMenu());
    }

    void LoadMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("Main Menu");
    }
}
