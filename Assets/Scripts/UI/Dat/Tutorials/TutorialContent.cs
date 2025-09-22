using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialContent : MonoBehaviour
{
    [Header("Components")]
    public Image Image;
    public TextMeshProUGUI Content;

    [Header("References")]
    public TutorialManager TutorialManager;

    public void SetupTutorialContent(TutorialStep tutorialStep)
    {
        Image.sprite = tutorialStep.ImageTutContent;
        Content.text = tutorialStep.TutContent;
    }
}
