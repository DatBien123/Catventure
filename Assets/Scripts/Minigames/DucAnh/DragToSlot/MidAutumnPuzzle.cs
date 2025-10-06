using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MidAutumnPuzzle : BaseMinigame
{
    public static MidAutumnPuzzle Instance { get; private set; }

    [SerializeField] private RectTransform UIWinPanel;
    [SerializeField] private int pointsTowin;
    [SerializeField] private int reward;

    [SerializeField] private UnityEvent playBackgroundMusic;
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
    public void Start()
    {
        Debug.Log("Chạy nhạc");
        playBackgroundMusic?.Invoke();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddPoint()
    {
        currentPoints++;
        CheckWin();
    }

    private void CheckWin()
    {
        if (currentPoints >= pointsTowin)
        {
            OnWinGame();
        }
    }

    private void OnWinGame()
    {
        StartCoroutine(OpenWinPanel());

        //Save
        Player.Coin += reward;
        SaveSystem.Save(Player, Player.Inventory);
    }

    IEnumerator OpenWinPanel()
    {
        yield return new WaitForSeconds(1.0f);
        victoryRewardScreen.ShowRewardFITB(reward, 3);
    }

    public override void ExitGame()
    {
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
