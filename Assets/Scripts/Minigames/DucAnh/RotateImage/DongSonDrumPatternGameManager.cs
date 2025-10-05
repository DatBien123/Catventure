using System.Collections;
using UnityEngine;

public class DongSonDrumPatternGameManager : BaseMinigame
{
    public static DongSonDrumPatternGameManager Instance { get; private set; }

    [Header("Game Reference")]
    [SerializeField] private SwordChest Chest;
    [SerializeField] private GameObject PuzzleLayers;
    [SerializeField] private int pointsTowin;
    [SerializeField] private int reward;
    [SerializeField] private GameObject MusicManager;

    private int currentPoints;

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

    public void ZoomInChest() {
        StartCoroutine(IEZoomInChest());
    }

    private IEnumerator IEZoomInChest() {
        int defaultVCamIndex = 0;
        int zoominVCamIndex = 1;

        CameraManager.Instance.SwitchCamera(zoominVCamIndex);

        DongSonDrumPatternUIManager.Instance.FadeTransition();

        yield return new WaitForSeconds(1);

        CameraManager.Instance.SwitchCamera(defaultVCamIndex);

        Chest.gameObject.SetActive(false);
        PuzzleLayers.SetActive(true);
    }


    public void AddPoint() {
        currentPoints++;
        CheckWin();
    }

    public void CheckWin() {
        if (currentPoints >= pointsTowin) {
            OnWinGame();
        }
    }

    void OnWinGame() {
        MusicManager.SetActive(false);
        DongSonDrumPatternUIManager.Instance.OpenWinPanel(reward);

        //Save
        Player.Coin += reward;
        SaveSystem.Save(Player, Player.Inventory);
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
