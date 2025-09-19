using UnityEngine;

public class DragDropCookingMinigame : DragDropMinigame
{
    public CookingManager cookingManager;
    public CookingRecipeSO recipe; // công thức món ăn ta muốn nấu 

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
        StartGame();
        
    }
    public override void StartGame()
    {
        base.StartGame();
        // Chuẩn bị dữ liệu để bắt đầu chạy các step tương ứng ở đây
        Debug.Log("Bắt đầu minigame nấu ăn");
        cookingManager.StartRecipe(recipe);
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
        victoryRewardScreen.ShowRewardDragDrop(recipe.dishResult.icon, recipe.dishResult.dishName, recipe.reward,3);
    }

}
