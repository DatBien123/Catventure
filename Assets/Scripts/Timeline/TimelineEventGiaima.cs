using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineEventGiaima : MonoBehaviour
{
    #region [New]
    public PlayableDirector director;   // Gán PlayableDirector vào đây

    [Header("References")]
    public GameObject CutSceneUI;

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
                        topic => topic.topicName == "Giải mã"
                    );
                    return topicFound;
            }
        }

        Debug.LogWarning("Không tìm thấy topic 'Giải mã' hoặc map không phải 'Hà Nội'");
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
                    topic => topic.topicName == "Giải mã"
                );

                if (topicFound.cutscene != null)
                {
                    return topicFound.cutscene;
                }
                else
                {
                    Debug.LogWarning("Topic 'Giải mã' không có cutscene!");
                }
            }
        }

        Debug.LogWarning("Không tìm thấy topic 'Giải mã' hoặc map không phải 'Hà Nội'");
        return null;
    }

    public void CompleteTopic()
    {
        MapInstance mapInstance = GetCurrentMapSelecting();
        if (!mapInstance.CompletedTopicsIndex.Contains(GetCurrentTopic().index))
        {
            mapInstance.CompletedTopicsIndex.Add(GetCurrentTopic().index);
        }
        // Mở khóa topic tiếp theo nếu có
        int nextTopicIndex = GetCurrentTopic().index + 1;
        if (nextTopicIndex < mapInstance.MapData.Data.topicList.Count &&
            !mapInstance.UnlockTopicsIndex.Contains(nextTopicIndex))
        {
            mapInstance.UnlockTopicsIndex.Add(nextTopicIndex);
        }

        List<MapInstance> ListMapLoaded = MapSaveSystem.Load();
        for (int i = 0; i < ListMapLoaded.Count; i++)
        {
            if (ListMapLoaded[i].MapData.Data.Name == mapInstance.MapData.Data.Name)
            {
                ListMapLoaded[i].isUnlock = mapInstance.isUnlock;
                ListMapLoaded[i].isSelected = mapInstance.isSelected;
                ListMapLoaded[i].UnlockTopicsIndex = mapInstance.UnlockTopicsIndex;
                ListMapLoaded[i].CompletedTopicsIndex = mapInstance.CompletedTopicsIndex;

                //Mo khoa map tiep theo
                if (ListMapLoaded[i].UnlockTopicsIndex.Count == ListMapLoaded[i].CompletedTopicsIndex.Count)
                {
                    ListMapLoaded[i + 1].isUnlock = true;
                    if (!ListMapLoaded[i + 1].UnlockTopicsIndex.Contains(0))
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

    }

    void OnTimelineStop(PlayableDirector obj)
    {
        CutSceneUI.SetActive(false);
    }
    #endregion
}