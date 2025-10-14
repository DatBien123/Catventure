using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FixSwordGameManager : BaseMinigame
{
    public static FixSwordGameManager Instance { get; private set; }

    [SerializeField] private GameObject MusicManager;
    [SerializeField] private AudioClip WinGameClip;
    [SerializeField] private int pointsTowin;
    [SerializeField] private int reward;
    [SerializeField] private GameObject CutSceneGameObject;

    [Header("Data Save")]
    public CharacterPlayer Player;

    private int currentPoints;

    protected override void OnEnable()
    {
        base.OnEnable();
        DragToSlotPuzzlePiece.onAddPoint += AddPoint;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DragToSlotPuzzlePiece.onAddPoint -= AddPoint;
    }

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
        AudioManager.instance.PlayPronunciation(WinGameClip);
        StartCoroutine(IEOpenCutScene());

        //Save
        Player.Coin += reward;
        SaveSystem.Save(Player, Player.Inventory);
    }

    IEnumerator IEOpenCutScene() {
        yield return new WaitForSeconds(2.0f);
        CutSceneGameObject.SetActive(true);
    }

    public void OpenWinPanel() {
        StartCoroutine(IEOpenWinPanel());
    }

    IEnumerator IEOpenWinPanel() {
        yield return new WaitForSeconds(1.0f);
        MusicManager.SetActive(false);
        victoryRewardScreen.ShowRewardFITB(reward, 3);
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
