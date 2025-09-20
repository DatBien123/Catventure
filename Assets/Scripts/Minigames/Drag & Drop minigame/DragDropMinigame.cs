using UnityEngine;

public class DragDropMinigame : BaseMinigame
{
    public override void StartGame()
    {
        base.StartGame();
        Debug.Log("DragDropMinigame started!");
        // logic khởi động riêng cho minigame kéo thả
    }

    public override void EndGame(bool success)
    {
        Debug.Log($"DragDropMinigame ended. Success = {success}");
        base.EndGame(success);
    }
    public override void GameOver()
    {
        base.GameOver();    
    }



}
