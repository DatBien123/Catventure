using UnityEngine;

public interface IMinigame
{
    void StartGame();
    void EndGame(bool success); 
    void GameOver();
    event System.Action<bool> OnGameEnd; // true = win, false = lose

}
