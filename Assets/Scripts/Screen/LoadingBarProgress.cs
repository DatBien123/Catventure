using System.Collections;
using UnityEngine;
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
        // Uncomment if you want to assign the slider in code
        // loadingBarSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        if (videoPlayer != null)
        {
            //videoPlayer.errorReceived += OnVideoError; // Add error handling
            //videoPlayer.Prepare();
            //videoPlayer.prepareCompleted += OnPrepared;

            videoPlayer.Play();
        }

        if (isEnableTransitionOnStart)
        {
            StartLoadSceneAsync(sceneName);
        }
    }

    //void OnPrepared(VideoPlayer vp)
    //{
    //    vp.Play();
    //}

    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError($"VideoPlayer Error: {message}");
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.errorReceived -= OnVideoError;
        }
    }

    Coroutine C_LoadScene;
    public void StartLoadSceneAsync(string sceneName)
    {
        if (C_LoadScene != null) StopCoroutine(C_LoadScene);
        C_LoadScene = StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Wait briefly to ensure video playback stabilizes
        yield return new WaitForSeconds(0.1f);

        float elapsedTime = 0.0f;

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);
        }

        // Start asynchronous scene loading with low priority
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevent immediate scene switch
        asyncLoad.priority = -1; // Lower priority to reduce main-thread impact

        float updateInterval = 0.0f; // Update slider every 0.1 seconds to reduce load
        float lastUpdateTime = 0.0f;

        while (elapsedTime < loadDuration || !asyncLoad.isDone)
        {
            elapsedTime += Time.deltaTime;

            // Update progress less frequently
            if (elapsedTime - lastUpdateTime >= updateInterval)
            {
                float asyncProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Async progress (0 to 1)
                float timeProgress = Mathf.Clamp01(elapsedTime / loadDuration); // Time-based progress
                float combinedProgress = Mathf.Min(asyncProgress, timeProgress); // Use the slower of the two

                if (loadingBarSlider != null)
                {
                    loadingBarSlider.value = combinedProgress;
                }

                Debug.Log($"Loading progress: {(combinedProgress * 100):F2}% (Async: {(asyncProgress * 100):F2}%, Time: {(timeProgress * 100):F2}%)");

                lastUpdateTime = elapsedTime;
            }

            // Allow scene activation only when both async loading is complete and duration is met
            if (asyncLoad.progress >= 0.9f && elapsedTime >= loadDuration)
            {
                asyncLoad.allowSceneActivation = true;
            }

            // Yield to give the main thread breathing room
            yield return new WaitForEndOfFrame();
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }

    public void StartLoadSceneNormal(string sceneName)
    {
        if (C_LoadScene != null) StopCoroutine(C_LoadScene);
        C_LoadScene = StartCoroutine(LoadSceneNormal(sceneName));
    }

    IEnumerator LoadSceneNormal(string sceneName)
    {
        float elapsedTime = 0.0f;

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);
        }

        float updateInterval = 0.1f; // Update slider every 0.1 seconds
        float lastUpdateTime = 0.0f;

        while (elapsedTime <= loadDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime - lastUpdateTime >= updateInterval)
            {
                float rate = elapsedTime / loadDuration;
                float progress = Mathf.Clamp01(rate / 0.9f);
                if (loadingBarSlider != null)
                {
                    loadingBarSlider.value = progress;
                }
                Debug.Log($"Loading progress: {(progress * 100):F2}%");
                lastUpdateTime = elapsedTime;
            }

            yield return new WaitForEndOfFrame();
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
        SceneManager.LoadScene(sceneName);
    }
}