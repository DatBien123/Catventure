using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragDropCookingMinigame : DragDropMinigame
{
    public CookingManager cookingManager;
    public CookingRecipeSO recipe; // công thức món ăn ta muốn nấu 
    public CharacterPlayer zera;

    public SO_MapSelecting MapSelecting;

     protected override void OnEnable()
    {
        base.OnEnable();
        cookingManager.onFinishCooking += ShowRewardUI;
    }
    protected override void OnDisable() {
        base.OnDisable();
        cookingManager.onFinishCooking -= ShowRewardUI;

    }
    public void Start()
    {
        //StartGame();
        
    }
    public void Setup(CookingRecipeSO p_recipe)
    {
        Debug.Log("Setup"); 
        recipe = p_recipe;
        StartGame();
    }
    public override void StartGame()
    {
        base.StartGame();
        // Chuẩn bị dữ liệu để bắt đầu chạy các step tương ứng ở đây
        Debug.Log("Bắt đầu minigame nấu ăn");
        cookingManager.StartRecipe(recipe);

        //Mo khoa minigame tiep theo
        //MapSelecting.Data.CurrentTopic.isCompleted = true;

        for (int i = 0; i < MapSelecting.Data.CurrentMapSelecting.Data.topicList.Count; i++)
        {
            if (MapSelecting.Data.CurrentMapSelecting.Data.topicList[i].index == MapSelecting.Data.CurrentTopic.index + 1)
            {
                var t = MapSelecting.Data.CurrentMapSelecting.Data.topicList[i];
                t.isUnlock = true;
                MapSelecting.Data.CurrentMapSelecting.Data.topicList[i] = t; // ghi lại vào list
                break;
            }
            if (MapSelecting.Data.CurrentMapSelecting.Data.topicList[i].index == MapSelecting.Data.CurrentTopic.index)
            {
                var t = MapSelecting.Data.CurrentMapSelecting.Data.topicList[i];
                t.isCompleted = true;
                MapSelecting.Data.CurrentMapSelecting.Data.topicList[i] = t; // ghi lại vào list
                break;
            }
        }


    }
    public override void EndGame(bool success)
    {
        base.EndGame(success);
        Debug.Log("End Game");
    }
    public override void GameOver()
    {
        base.GameOver();    

    }
    public void ShowRewardUI()
    {
        EndGame(true);
        countDownTimer.StopCountDown();
        foreach (var req in recipe.dishResult.ingredients)
        {
            zera.Inventory.RemoveItem(req.ingredient, req.requiredAmount);
        }
        zera.Inventory.AddItem(new ItemInstance(recipe.dishResult, 1 , false));
        victoryRewardScreen.ShowRewardDragDrop(recipe.dishResult.icon, recipe.dishResult.dishName, recipe.reward,3);
    }
    public override void Replay()
    {
        base.Replay();
    }
    public override void ExitGame()
    {
        base.ExitGame();
        // Thoát game về home menu
        Debug.Log("Quay về chọn món");
        popupUI.ShowConfirm(
        "MENU",
        "Bạn muốn về Menu chọn món sao?",
        yesCallback: () => {
            GoToScene("Minigame_Drag&DropCooking");
        },
        noCallback: () => {
        }
        );
    }
    


}
