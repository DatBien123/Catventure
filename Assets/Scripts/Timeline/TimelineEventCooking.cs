using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

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
    }
    public MapInstance GetCurrentMapSelecting()
    {
        return
        MapSaveSystem.GetSelected();

    }

    public TimelineAsset GetCorrespondingCutScene()
    {
        MapInstance currentMapSelected = GetCurrentMapSelecting();
        Debug.Log("Map selecting is: " + currentMapSelected.MapData.Data.Name);
        if (currentMapSelected != null)
        {
            if (currentMapSelected.MapData.Data.Name == "Hà Nội")
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
    }

    void OnTimelineStop(PlayableDirector obj)
    {
        CutSceneUI.SetActive(false);
    }
    #endregion
}