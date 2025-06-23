using System.Collections;
using TMPro;
using UnityEngine;

public class GlitchTextEffect : MonoBehaviour
{
    public TMP_Text textComponent;
    public float glitchInterval = 0.1f;
    public string[] glitchStrings;

    private string originalText;

    void Start()
    {
        originalText = textComponent.text;
        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            string glitched = glitchStrings[Random.Range(0, glitchStrings.Length)];
            textComponent.text = glitched;
            yield return new WaitForSeconds(glitchInterval);
            textComponent.text = originalText;
            yield return new WaitForSeconds(glitchInterval * 2f);
        }
    }
}
