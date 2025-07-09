using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSequence : MonoBehaviour
{
    [SerializeField] private float delayBeforeMenu = 7f;
    [SerializeField] private string menuSceneName = "Main Menu";

    private void Start()
    {
        Debug.Log("BootSequence started. Waiting to load menu...");

        // Optional audio
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        // Wait before loading menu
        Invoke(nameof(LoadMainMenu), delayBeforeMenu);
    }

    private void LoadMainMenu()
    {
        Debug.Log("Attempting to load scene: " + menuSceneName);
        SceneManager.LoadScene(menuSceneName);
    }
}
