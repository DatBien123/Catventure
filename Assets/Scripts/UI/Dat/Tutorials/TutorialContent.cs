using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialContent : MonoBehaviour
{
    [Header("Components")]
    public Image Image;
    public TextMeshProUGUI Content;
    public RectTransform RectTransform;

    [Header("References")]
    public TutorialManager TutorialManager;

    public void SetupTutorialContent(TutorialStep tutorialStep)
    {
        Image.sprite = tutorialStep.ImageTutContent;
        Content.text = tutorialStep.TutContent;

        ShowText(Content.text);
    }


    #region [Typer Writer Text Effect]
    public float delay = 0.01f;

    public void ShowText(string fullText)
    {
        StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string text)
    {
        Content.text = "";
        foreach (char c in text)
        {
            Content.text += c;
            yield return new WaitForSeconds(delay);
        }
    }

    #endregion
}
