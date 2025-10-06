using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class WordsSpyGameManager : BaseMinigame
{
    public static WordsSpyGameManager Instance { get; private set; }

    [SerializeField] private int reward;

    [Header("Data Save")]
    public CharacterPlayer Player;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        GameEvents.OnBoardCompleted += ShowWinPopUp;
    }

    protected override void OnDisable() {
        base.OnDisable();
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
        victoryRewardScreen.ShowRewardFITB(reward, 3);
    }

    public void OnWinGame() {
        //Save
        Player.Coin += reward;
        SaveSystem.Save(Player, Player.Inventory);

        GameEvents.OnBoardCompletedMethod();
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
