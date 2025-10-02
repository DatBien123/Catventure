using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DongSonDrumPatternUIManager : MonoBehaviour
{
    public static DongSonDrumPatternUIManager Instance { get; private set; }

    [SerializeField] private RectTransform UIWinPanel;
    [SerializeField] private VictoryRewardScreen victoryRewardScreen;
    [SerializeField] private Image fadePanel; 

    private float fadeDuration = 1.0f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void FadeTransition() {
        StartCoroutine(IEFadeTransition());
    }

    private IEnumerator IEFadeTransition() {
        float elapsedTime = 0f;
        fadePanel.gameObject.SetActive(true);
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            panelColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadePanel.color = panelColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            panelColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadePanel.color = panelColor;
            yield return null;
        }

        panelColor.a = 0f; 
        fadePanel.color = panelColor;
        fadePanel.gameObject.SetActive(false); 
    }

    public void OpenWinPanel(int reward) {
        StartCoroutine(IEOpenWinPanel(reward));
    }

    private IEnumerator IEOpenWinPanel(int reward) {
        yield return new WaitForSeconds(1.0f);
        victoryRewardScreen.ShowRewardFITB(reward, 3);
    }


}
