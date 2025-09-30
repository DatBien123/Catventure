using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class StoryManager : MonoBehaviour
{
    public List<SO_Story> Stories = new List<SO_Story>();
    public Story CurrentStory;
    public StoryPageData CurrentStoryPageData;
    public int CurrentIndex = 0;
    public bool isApplyNextPage = false;
    public bool isPlayingStory = false;

    [Header("References")]
    public GameObject StoryPanel;
    public StoryGenerator StoryGenerator;
    public VideoPlayer VideoPlayer;
    public AudioManager AudioManager;
    public Image ImageLayer;

    private void Awake()
    {
        StoryGenerator = GetComponent<StoryGenerator>();
    }

    private void Start()
    {
        StartCoroutine(PreloadAllClips());
        VideoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        StartCoroutine(FadeInImageLayer());
        StartCoroutine(FadeOutImageLayer());
    }

    IEnumerator PreloadAllClips()
    {
        foreach (var story in Stories)
        {
            foreach (var storyPageData in story.Data.StoryPageDatas)
            {
                VideoPlayer.clip = storyPageData.VideoClip;
                VideoPlayer.Prepare();
                yield return new WaitUntil(() => VideoPlayer.isPrepared);
                Debug.Log($"{storyPageData.VideoClip.name} đã sẵn sàng phát!");
            }
        }
    }

    Coroutine C_PlayStory;
    public void StartPlayStory(SO_Story storyData)
    {
        if (C_PlayStory != null) StopCoroutine(C_PlayStory);
        C_PlayStory = StartCoroutine(PlayStory(storyData));
    }

    IEnumerator PlayStory(SO_Story storyData)
    {
        // Setup
        isPlayingStory = true;
        StoryPanel.SetActive(true);
        CurrentIndex = 0;
        CurrentStoryPageData = storyData.Data.StoryPageDatas[CurrentIndex];

        while (isPlayingStory)
        {
            isApplyNextPage = false;
            VideoPlayer.clip = CurrentStoryPageData.VideoClip;
            yield return StartCoroutine(FadeInImageLayer());
            VideoPlayer.Play();
            yield return new WaitForSeconds(.5f);
            yield return StartCoroutine(FadeOutImageLayer());
            Debug.Log($"phát!");
            yield return new WaitWhile(() => isApplyNextPage == false);
            // Fade in and out ImageLayer
        }
        yield return StartCoroutine(FadeInImageLayer());
        yield return StartCoroutine(FadeOutImageLayer());
        VideoPlayer.Stop();
        StoryPanel.SetActive(false);
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

    public void OnNextStoryPage()
    {
        CurrentIndex++;

        if (CurrentIndex < CurrentStory.StoryData.Data.StoryPageDatas.Count)
        {
            CurrentStoryPageData = CurrentStory.StoryData.Data.StoryPageDatas[CurrentIndex];
            isApplyNextPage = true;
        }
        else
        {
            isApplyNextPage = true;
            isPlayingStory = false;
        }
    }

    public void OnPreviousStoryPage()
    {
        CurrentIndex--;
        if (CurrentIndex >= 0)
        {
            CurrentStoryPageData = CurrentStory.StoryData.Data.StoryPageDatas[CurrentIndex];
            isApplyNextPage = true;
        }
    }
}