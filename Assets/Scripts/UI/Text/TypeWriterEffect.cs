using System.Collections;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float delay = 0.05f;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        textMeshPro.ForceMeshUpdate();
        var textInfo = textMeshPro.textInfo;
        textMeshPro.maxVisibleCharacters = 0;

        int totalVisibleCharacters = textInfo.characterCount;
        int counter = 0;

        while (counter <= totalVisibleCharacters)
        {
            textMeshPro.maxVisibleCharacters = counter;
            counter++;
            yield return new WaitForSeconds(delay);
        }
    }
}
