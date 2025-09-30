using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public enum ELoadType
{
    LoadAsync,
    LoadNormal
}
public class LoadingBarProgress : MonoBehaviour
{
    [Header("Loading bar")]
    public Slider loadingBarSlider;
    public float loadDuration = 5.0f;
    public bool isEnableTransitionOnStart = false;

    public string sceneName;

    public VideoPlayer videoPlayer;
    private void Awake()
    {
        //loadingBarSlider = GetComponent<Slider>();
    }
    private void Start()
    {
        if(videoPlayer != null)
        {
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += OnPrepared;
        }


        if (isEnableTransitionOnStart)
        StartLoadSceneNormal(sceneName);
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    Coroutine C_LoadScene;
    //public void StartLoadSceneAsync(string sceneName)
    //{
    //    if (C_LoadScene != null) StopCoroutine(C_LoadScene);
    //    C_LoadScene = StartCoroutine(LoadSceneAsync(sceneName));
    //}
    //IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

    //    while (!operation.isDone )
    //    {
    //        float progress = Mathf.Clamp01(operation.progress / 0.9f);
    //        loadingBarSlider.value = progress;
    //        Debug.Log("Loading progress: " + (progress * 100) + "%");
    //        yield return null;
    //    }
    //}
    public void StartLoadSceneNormal(string sceneName)
    {
        if (C_LoadScene != null) StopCoroutine(C_LoadScene);
        C_LoadScene = StartCoroutine(LoadSceneNormal(sceneName));
    }
    IEnumerator LoadSceneNormal(string sceneName)
    {
        float elapsedTime = 0.0f;

        videoPlayer.gameObject.SetActive(true);
        //videoPlayer.Play();

        while (elapsedTime <= loadDuration)
        {
            float rate = elapsedTime / loadDuration;
            float progress = Mathf.Clamp01(rate / 0.9f);
            if(loadingBarSlider)
            loadingBarSlider.value = progress;
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            elapsedTime += Time.deltaTime;
                yield return null;
        }
        videoPlayer.Stop();
        SceneManager.LoadScene(sceneName);
    }
}
