using System.Collections;
using UnityEngine;

public class FixSwordGameManager : BaseMinigame
{
    public static FixSwordGameManager Instance { get; private set; }

    [SerializeField] private RectTransform UIWinPanel;
    [SerializeField] private VictoryRewardScreen VictoryRewardScreen;
    [SerializeField] private GameObject MusicManager;
    [SerializeField] private int pointsTowin;
    [SerializeField] private int reward;

    [Header("Data Save")]
    public CharacterPlayer Player;

    private int currentPoints;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void AddPoint() { 
        currentPoints++;
        CheckWin();
    }

    private void CheckWin() {
        if (currentPoints >= pointsTowin) {
            OnWinGame();
        }
    }

    private void OnWinGame() {
        MusicManager.SetActive(false);
        StartCoroutine(OpenWinPanel());

        //Save
        Player.Coin += reward;
        SaveSystem.Save(Player, Player.Inventory);
    }

    IEnumerator OpenWinPanel() {
        yield return new WaitForSeconds(1.0f);
        VictoryRewardScreen.ShowRewardFITB(reward, 3);
    }

    public override void ExitGame() {
        base.ExitGame();
        // Thoát game về home menu
        Debug.Log("Quay về Home Menu");
        popupUI.ShowConfirm(
        "MENU",
        "Bạn muốn về Home Menu sao?",
        yesCallback: () => {
            GoToScene("Home Scene");
        },
        noCallback: () => {
        }
        );
    }
}
