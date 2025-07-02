using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingBarProgress : MonoBehaviour
{
    [Header("Loading bar")]
    public Slider loadingBarSlider;
    //public float lerpRaito = 5.0f;
    public float loadDuration = 5.0f;

    [Header("On Finish Action")]
    public UnityEvent onFinishedLoad;
    private void Awake()
    {
        loadingBarSlider = GetComponent<Slider>();
    }
    private void Start()
    {
        StartLoad(loadDuration);
    }

    Coroutine C_StartLoad;

    public void StartLoad(float duration)
    {
        if(C_StartLoad != null) StopCoroutine(C_StartLoad);
        C_StartLoad = StartCoroutine(Load(duration));
    } 
    IEnumerator Load(float loadingDuration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= loadingDuration)
        {
            loadingBarSlider.value += elapsedTime * Time.deltaTime / loadingDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onFinishedLoad?.Invoke();

    }
}
