using UnityEngine;

public class WinPopUp : MonoBehaviour
{
    [SerializeField] private VictoryRewardScreen VictoryRewardScreen;
    [SerializeField] private int reward;

    private void OnEnable() {
        GameEvents.OnBoardCompleted += ShowWinPopUp;
    }

    private void OnDisable() {
        GameEvents.OnBoardCompleted -= ShowWinPopUp;

    }

    private void ShowWinPopUp() {
        VictoryRewardScreen.ShowRewardFITB(reward, 3);
    }

    public void LoadNextLevel() {
        GameEvents.LoadNextLevelMethod();
    }
}
