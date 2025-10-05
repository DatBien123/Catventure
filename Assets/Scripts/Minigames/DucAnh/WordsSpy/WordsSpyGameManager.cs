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
