using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    public Button creditsButton; // Assign in Inspector or via GetComponent

    void Start()
    {
        if (creditsButton == null)
            creditsButton = GetComponent<Button>();

        creditsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(() => SceneManager.LoadScene("Credits"));
    }
}
