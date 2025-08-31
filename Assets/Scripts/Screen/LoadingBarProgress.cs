using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public string sceneName;
    public ELoadType loadType;

    private void Awake()
    {
        //loadingBarSlider = GetComponent<Slider>();
    }
    private void Start()
    {

         StartLoadSceneNormal(sceneName, loadDuration);
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
    public void StartLoadSceneNormal(string sceneName, float duration)
    {
        if (C_LoadScene != null) StopCoroutine(C_LoadScene);
        C_LoadScene = StartCoroutine(LoadSceneNormal(sceneName, duration));
    }
    IEnumerator LoadSceneNormal(string sceneName, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            float rate = elapsedTime / duration;
            float progress = Mathf.Clamp01(rate / 0.9f);
            if(loadingBarSlider)
            loadingBarSlider.value = progress;
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            elapsedTime += Time.deltaTime;
                yield return null;
        }
       SceneManager.LoadScene(sceneName);
    }
}
