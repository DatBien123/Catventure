using UnityEngine;
using UnityEngine.UI;

public class RhythmGameManager : BaseMinigame
{
    public static RhythmGameManager Instance { get; private set; }

    [SerializeField] private Text perfectCountText;
    [SerializeField] private Text greatCountText;
    [SerializeField] private Text goodCountText;
    [SerializeField] private Text missCountText;
    [SerializeField] private Text maxComboCountText;
    [SerializeField] private Text totalScoreText;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void OnFinishGame (float reward, int star, int perfectCount, int greatCount, int goodCount, int missCount, int maxComboCount, int totalScore) {
        perfectCountText.text += perfectCount;
        greatCountText.text += greatCount;
        goodCountText.text += goodCount;
        missCountText.text += missCount;
        maxComboCountText.text += maxComboCount;
        totalScoreText.text += totalScore;
        victoryRewardScreen.ShowRewardFITB(reward, star);
    }

    public override void ExitGame() {
        base.ExitGame();
        // Thoát game về home menu
        Debug.Log("Quay về Home Scene");
        popupUI.ShowConfirm(
        "MENU",
        "Bạn muốn thoát game?",
        yesCallback: () => {
            GoToScene(Constants.HOME_SCENE_NAME);
        },
        noCallback: () => {
        }
        );
    }
}
