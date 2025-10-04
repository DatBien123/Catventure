using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EStoryType
{
    None = 0,
    Navigate
}
public class StoryManager : MonoBehaviour
{
    public List<StoryInstance> Stories = new List<StoryInstance>();
    public Story CurrentStory;
    public StoryPageData CurrentStoryPageData;
    public int CurrentIndex = 0;
    public bool isApplyNextPage = false;
    public bool isPlayingStory = false;

    [Header("References")]
    public GameObject StoryPanel;
    public StoryGenerator StoryGenerator;
    public Image StoryImage;
    public AudioManager AudioManager;
    public Image ImageLayer;
    public Sound CurrentSound;
    private void Awake()
    {
        CurrentSound = new Sound();
        CurrentSound.source = gameObject.AddComponent<AudioSource>();
        StorySaveSystem.Load(this); // Tải trạng thái isUnlock khi khởi tạo
    }

    Coroutine C_PlayStory;
    public void StartPlayStory(StoryInstance story)
    {
        if (C_PlayStory != null) StopCoroutine(C_PlayStory);
        C_PlayStory = StartCoroutine(PlayStory(story));
    }
    #region [Actions]
    IEnumerator PlayStory(StoryInstance story)
    {

        // Setup
        isPlayingStory = true;
        StoryPanel.SetActive(true);
        CurrentIndex = 0;
        CurrentStoryPageData = story.StoryData.Data.StoryPageDatas[CurrentIndex];

        yield return StartCoroutine(FadeInImageLayer());
        yield return StartCoroutine(FadeOutImageLayer());

        while (isPlayingStory)
        {
            isApplyNextPage = false;
            //yield return new WaitForSeconds(.5f);
            StoryImage.sprite = CurrentStoryPageData.StoryImage;

            int currentAudioIndex = 0;

            while(currentAudioIndex < CurrentStoryPageData.StoryAudioClips.Count)
            {
                CurrentSound.source.clip = CurrentStoryPageData.StoryAudioClips[currentAudioIndex];
                CurrentSound.source.volume = 1;
                CurrentSound.source.pitch = 1;
                CurrentSound.source.loop = false;
                CurrentSound.source.Play();

                Debug.Log($"phát!");
                //yield return new WaitWhile(() => isApplyNextPage == false);
                yield return new WaitForSeconds(CurrentStoryPageData.StoryAudioClips[currentAudioIndex].length);
                currentAudioIndex++;
            }


            if (CurrentStoryPageData.isDelay)
            {
                yield return StartCoroutine(FadeInImageLayer());
                OnNextStoryPage();
                yield return StartCoroutine(FadeOutImageLayer());
            }
            else {
                OnNextStoryPage();
            }

            // Fade in and out ImageLayer
        }
        yield return StartCoroutine(FadeInImageLayer());
        StoryPanel.SetActive(false);
        StoryImage.sprite = null;
        StoryGenerator.StartStopReadStory();

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

        if (CurrentIndex < CurrentStory.StoryData.StoryData.Data.StoryPageDatas.Count)
        {
            CurrentStoryPageData = CurrentStory.StoryData.StoryData.Data.StoryPageDatas[CurrentIndex];
            StoryImage.sprite = CurrentStoryPageData.StoryImage;
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
            CurrentStoryPageData = CurrentStory.StoryData.StoryData.Data.StoryPageDatas[CurrentIndex];
            StoryImage.sprite = CurrentStoryPageData.StoryImage;
            isApplyNextPage = true;
        }
    }
    #endregion

    #region [Data]
    //Data
    // Phương thức để unlock story (gọi khi cần)
    public void UnlockStory(string storyID, bool isUnlock)
    {
        StoryInstance storyInstance = Stories.Find(s => s.StoryData.Data.ID == storyID);
        if (storyInstance != null)
        {
            storyInstance.isUnlock = isUnlock;
            StorySaveSystem.Save(this); // Lưu ngay khi thay đổi
        }
    }
    #endregion
}