using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineEventCooking : MonoBehaviour
{
    #region [New]
    public PlayableDirector director;   // Gán PlayableDirector vào đây

    [Header("References")]
    public SO_MapSelecting MapSelecting;
    public FoodSelection FoodSelection;
    public GameObject CutSceneUI;

    [Header("Foods")]
    public List<DishSO> ListFood;

    private void Start()
    {
        director.playableAsset = MapSelecting.Data.CurrentTopic.cutscene;
        director.Play();
        FoodSelection.allFoods.Clear();
        if(MapSelecting.Data.CurrentMapSelecting.Data.Name == "Hà Nội")
        {
            foreach(var food in ListFood)
            {
                if(food.dishName == "Phở")
                {
                    FoodSelection.allFoods.Add(food);
                }
            }


        }
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
