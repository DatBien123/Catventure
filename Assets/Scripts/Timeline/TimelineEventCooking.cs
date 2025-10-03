using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;  
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineEventCooking : MonoBehaviour
{
    #region [New]
    public PlayableDirector director;   // Gán PlayableDirector vào đây

    [Header("References")]
    public FoodSelection FoodSelection;
    public GameObject CutSceneUI;

    [Header("Foods")]
    public List<DishSO> ListFood;

    public List<SO_Map> AllMaps;

    [Header("Skip button")]
    public Button Skip_Button;
    private void Start()
    {
        director.playableAsset = GetCorrespondingCutScene();
        director.Play();
        FoodSelection.allFoods.Clear();
        if (GetCurrentMapSelecting().MapData.Data.Name == "Hà Nội")
        {
            foreach (var food in ListFood)
            {
                if (food.dishName == "Phở")
                {
                    FoodSelection.allFoods.Add(food);
                }
            }
        }

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
                if (!currentMapSelected.CompletedTopicsIndex.Contains(0))
                {
                    Topic topicFound = currentMapSelected.MapData.Data.topicList.Find(
                        topic => topic.topicName == "Nấu Phở"
                    );
                    return topicFound;
                }

                if (!currentMapSelected.CompletedTopicsIndex.Contains(1))
                {
                    Topic topicFound = currentMapSelected.MapData.Data.topicList.Find(
                        topic => topic.topicName == "Nấu Cháo"
                    );
                    return topicFound;
                }
                if (!currentMapSelected.CompletedTopicsIndex.Contains(2))
                {
                    Topic topicFound = currentMapSelected.MapData.Data.topicList.Find(
                        topic => topic.topicName == "Nấu Cơm"
                    );


                    return topicFound;
                }

            }
        }

        Debug.LogWarning("Không tìm thấy topic 'Nấu phở' hoặc map không phải 'Hà Nội'");
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
                    topic => topic.topicName == "Nấu Phở"
                );

                if (topicFound.cutscene != null)
                {
                    return topicFound.cutscene;
                }
                else
                {
                    Debug.LogWarning("Topic 'Nấu phở' không có cutscene!");
                }
            }
        }

        Debug.LogWarning("Không tìm thấy topic 'Nấu phở' hoặc map không phải 'Hà Nội'");
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
        for(int i = 0;i<ListMapLoaded.Count;i ++)
        {
            if (ListMapLoaded[i].MapData.Data.Name == mapInstance.MapData.Data.Name)
            {
                ListMapLoaded[i].isUnlock = mapInstance.isUnlock;
                ListMapLoaded[i].isSelected = mapInstance.isSelected;
                ListMapLoaded[i].UnlockTopicsIndex = mapInstance.UnlockTopicsIndex;
                ListMapLoaded[i].CompletedTopicsIndex = mapInstance.CompletedTopicsIndex;

                //Mo khoa map tiep theo
                if(ListMapLoaded[i].UnlockTopicsIndex.Count == ListMapLoaded[i].CompletedTopicsIndex.Count)
                {
                    ListMapLoaded[i + 1].isUnlock = true;
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