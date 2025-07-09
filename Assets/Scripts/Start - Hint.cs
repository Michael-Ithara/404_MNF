using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public string sceneToLoad = "Level 1_2";
    public Button button;

    void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
        }
        else
        {
            Debug.LogError("GoToLevelButton: No Button component found or assigned.");
        }
    }
}
