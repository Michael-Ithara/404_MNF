using TMPro;
using UnityEngine;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;
    public string fullText;
    private string currentText = "";
    public TextMeshProUGUI textComponent;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
