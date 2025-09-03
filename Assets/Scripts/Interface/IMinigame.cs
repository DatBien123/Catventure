using UnityEngine;

public interface IMinigame
{
    void StartGame();
    void EndGame(bool success); 
    void GameOver();
}
