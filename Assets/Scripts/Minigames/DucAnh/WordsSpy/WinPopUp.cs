using System.Collections;
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
        StartCoroutine(IEOpenWinPanel(reward));
    }

    public void LoadNextLevel() {
        GameEvents.LoadNextLevelMethod();
    }

    private IEnumerator IEOpenWinPanel(int reward) {
        yield return new WaitForSeconds(1.0f);
        VictoryRewardScreen.ShowRewardFITB(reward, 3);
    }
}
