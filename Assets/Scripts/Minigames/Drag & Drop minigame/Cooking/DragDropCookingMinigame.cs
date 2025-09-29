using UnityEngine;
using UnityEngine.SceneManagement;

public class DragDropCookingMinigame : DragDropMinigame
{
    public CookingManager cookingManager;
    public CookingRecipeSO recipe; // công thức món ăn ta muốn nấu 
    public CharacterPlayer zera;
     protected override void OnEnable()
    {
        base.OnEnable();
        cookingManager.onFinishCooking += ShowRewardUI;
        UIEventSystem.Register("Quit",Quit);
    }
    protected override void OnDisable() {
        base.OnDisable();
        cookingManager.onFinishCooking -= ShowRewardUI;
        UIEventSystem.Unregister("Quit", Quit);

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
        AudioManager.instance.StopAllSounds();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene("Home Scene");

    }

}
