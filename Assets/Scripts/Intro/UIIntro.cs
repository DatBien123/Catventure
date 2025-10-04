using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIIntro : MonoBehaviour
{
    public GameObject LoadingUI;

    Coroutine C_StartIntro;

    public float delayTime = 3.0f;

    public TextMeshProUGUI Text;
    public Image ImageLayer;

    public VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer.Prepare();
    }
    void Start()
    {
        StartIntro();
        Text.transform.localScale = Vector3.zero; // Start from zero scale
        Text.transform.DOScale(1.1f, .5f) // Zoom to 1.1f slightly overshooting
            .SetEase(Ease.OutBack) // Adds a back effect for the overshoot
            .OnComplete(() => Text.transform.DOScale(1f, 1f)); // Settle back to 1f
    }
    public void StartIntro()
    {
        if(C_StartIntro != null)StopCoroutine(C_StartIntro);
        C_StartIntro = StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return StartCoroutine(FadeOutImageLayer());
        yield return new WaitForSeconds(delayTime);
        yield return StartCoroutine(FadeInImageLayer());
        this.gameObject.SetActive(false);

        LoadingUI.gameObject.SetActive(true);

    }

    private IEnumerator FadeInImageLayer()
    {
        // Ensure ImageLayer starts with alpha 0
        Color startColor = ImageLayer.color;
        startColor.a = 0f;
        ImageLayer.color = startColor;

        // Fade in (alpha 0 to 1)
        float elapsedTime = 0f;
        float fadeDuration = 0.75f; // Duration for fade-in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            ImageLayer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        ImageLayer.color = new Color(startColor.r, startColor.g, startColor.b, 1f); // Ensure alpha is exactly 1
    }

    private IEnumerator FadeOutImageLayer()
    {
        // Ensure ImageLayer starts with alpha 1
        Color startColor = ImageLayer.color;
        startColor.a = 1f;
        ImageLayer.color = startColor;

        // Fade out (alpha 1 to 0)
        float elapsedTime = 0f;
        float fadeDuration = 0.75f; // Duration for fade-out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            ImageLayer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        ImageLayer.color = new Color(startColor.r, startColor.g, startColor.b, 0f); // Ensure alpha is exactly 0
    }
}
