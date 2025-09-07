using UnityEngine;

public class WinPopUp : MonoBehaviour
{
    public GameObject winPopUp;

    private void Awake() {
        winPopUp.SetActive(false);
    }

    private void OnEnable() {
        GameEvents.OnBoardCompleted += ShowWinPopUp;
    }

    private void OnDisable() {
        GameEvents.OnBoardCompleted -= ShowWinPopUp;

    }

    private void ShowWinPopUp() {
        winPopUp.SetActive(true);
    }

    public void LoadNextLevel() {
        GameEvents.LoadNextLevelMethod();
    }
}
