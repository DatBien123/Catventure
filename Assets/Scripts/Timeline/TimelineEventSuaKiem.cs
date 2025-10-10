using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineEventSuaKiem : MonoBehaviour
{
    #region [New]
    public PlayableDirector director;   // Gán PlayableDirector vào đây

    [Header("References")]
    public GameObject CutSceneUI;
    [SerializeField] private GameObject MusicManager;

    [Header("Skip button")]
    public Button Skip_Button;
    private void Start()
    {
        director.playableAsset = GetCorrespondingCutScene();
        director.Play();

        Skip_Button.onClick.AddListener(() =>
        {
            director.Stop();
        });
    }
    public MapInstance GetCurrentMapSelecting()
    {
        return MapSaveSystem.GetSelected();
    }

    public Topic GetCurrentTopic()
    {
        MapInstance currentMapSelected = GetCurrentMapSelecting();
        if (currentMapSelected != null)
        {
            if (currentMapSelected.MapData.Data.Name == "Hà Nội" || currentMapSelected.MapData.Data.Name == "Hạ Long")
            {
                Topic topicFound = currentMapSelected.MapData.Data.topicList.Find(
                    topic => topic.topicName == "Sửa kiếm"
                );
                return topicFound;
            }
        }

        Debug.LogWarning("Không tìm thấy topic 'Sửa kiếm' hoặc map không phải 'Hà Nội'");
        return new Topic();
    }

    public TimelineAsset GetCorrespondingCutScene()
    {
        MapInstance currentMapSelected = GetCurrentMapSelecting();
        Debug.Log("Map selecting is: " + currentMapSelected.MapData.Data.Name);
        if (currentMapSelected != null)
        {
            if (currentMapSelected.MapData.Data.Name == "Hà Nội" || currentMapSelected.MapData.Data.Name == "Hạ Long")
            {
                Topic topicFound = currentMapSelected.MapData.Data.topicList.Find(
                    topic => topic.topicName == "Sửa kiếm"
                );

                if (topicFound.cutscene != null)
                {
                    return topicFound.cutscene;
                }
                else
                {
                    Debug.LogWarning("Topic 'Sửa kiếm' không có cutscene!");
                }
            }
        }

        Debug.LogWarning("Không tìm thấy topic 'Sửa kiếm' hoặc map không phải 'Hà Nội'");
        return null;
    }

    public void CompleteTopic()
    {
        MapInstance mapSelecting = GetCurrentMapSelecting();
        if (!mapSelecting.CompletedTopicsIndex.Contains(GetCurrentTopic().index))
        {
            mapSelecting.CompletedTopicsIndex.Add(GetCurrentTopic().index);
        }
        // Mở khóa topic tiếp theo nếu có
        int nextTopicIndex = GetCurrentTopic().index + 1;
        if (nextTopicIndex < mapSelecting.MapData.Data.topicList.Count &&
            !mapSelecting.UnlockTopicsIndex.Contains(nextTopicIndex))
        {
            mapSelecting.UnlockTopicsIndex.Add(nextTopicIndex);
        }

        List<MapInstance> ListMapLoaded = MapSaveSystem.Load();
        for (int i = 0; i < ListMapLoaded.Count; i++)
        {
            if (ListMapLoaded[i].MapData.Data.Name == mapSelecting.MapData.Data.Name)
            {
                ListMapLoaded[i].isUnlock = mapSelecting.isUnlock;
                ListMapLoaded[i].isSelected = mapSelecting.isSelected;
                ListMapLoaded[i].UnlockTopicsIndex = mapSelecting.UnlockTopicsIndex;
                ListMapLoaded[i].CompletedTopicsIndex = mapSelecting.CompletedTopicsIndex;

                //Mo khoa map tiep theo
                if (ListMapLoaded[i].UnlockTopicsIndex.Count == ListMapLoaded[i].CompletedTopicsIndex.Count)
                {
                    ListMapLoaded[i + 1].isUnlock = true;

                    if(!ListMapLoaded[i + 1].UnlockTopicsIndex.Contains(0))
                    ListMapLoaded[i + 1].UnlockTopicsIndex.Add(0);
                }
            }
        }
        MapSaveSystem.Save(ListMapLoaded);
    }

    void OnEnable()
    {
        // Đăng ký event
        director.played += OnTimelinePlay;
        director.stopped += OnTimelineStop;
    }

    void OnDisable()
    {
        // Hủy đăng ký event (tránh memory leak)
        director.played -= OnTimelinePlay;
        director.stopped -= OnTimelineStop;
    }

    void OnTimelinePlay(PlayableDirector obj)
    {
        CutSceneUI.SetActive(true);
        CompleteTopic();
        MusicManager.SetActive(false);

        //GameSession.Instance.SetLastScene("FixSword");
    }

    void OnTimelineStop(PlayableDirector obj)
    {
        CutSceneUI.SetActive(false);
        MusicManager.SetActive(true);
    }
    #endregion
}