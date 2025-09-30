using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class UIIntro : MonoBehaviour
{
    public GameObject LoadingUI;

    Coroutine C_StartIntro;

    public float delayTime = 3.0f;

    public TextMeshProUGUI Text;

    public VideoPlayer videoPlayer;
    void Start()
    {
        StartIntro();
        Text.transform.localScale = Vector3.zero; // Start from zero scale
        Text.transform.DOScale(1.1f, 0.3f) // Zoom to 1.1f slightly overshooting
            .SetEase(Ease.OutBack) // Adds a back effect for the overshoot
            .OnComplete(() => Text.transform.DOScale(1f, 0.2f)); // Settle back to 1f
    }
    public void StartIntro()
    {
        if(C_StartIntro != null)StopCoroutine(C_StartIntro);
        C_StartIntro = StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(delayTime);
        this.gameObject.SetActive(false);
        LoadingUI.gameObject.SetActive(true);
    }
}
