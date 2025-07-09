using UnityEngine;
using UnityEngine.UI;

public class ExitButtonScript : MonoBehaviour
{
    public Button quitButton;

    void Start()
    {
        if (quitButton == null)
            quitButton = GetComponent<Button>();

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
            Debug.Log("Game Quit");
        });
    }
}
