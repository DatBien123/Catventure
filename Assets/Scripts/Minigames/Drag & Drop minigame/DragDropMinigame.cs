using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
public class DragDropMinigame : MonoBehaviour, IMinigame
{
    public GameOverScreen gameOverScreen;
    public VictoryRewardScreen victoryRewardScreen;
    public Button tapToContinueButton; // Gán trong Inspector
    public GameObject floatingTextPrefab;
    public Canvas canvas;
    [SerializeField] protected GameObject inputBlocker;
    [SerializeField] protected HealthSystem health; // hệ thống máu
    [SerializeField] protected CountDownTimerSystem countDownTimer;
    // Tutorial của Drag & Drop minigame
    public HandTutorial hand;

    private void Awake()
    {
    }
    public void Start()
    {
        StartGame();
    }

    public virtual void StartGame()
    {

    }

    public virtual void EndGame(bool success)
    {
        countDownTimer.StopCountDown();

    }


    public void GameOver()
    {
        gameOverScreen.Setup();
    }
    public void ShowTapToContinue() // hàm xử lý bấm để tiếp tục
    {
        tapToContinueButton.gameObject.SetActive(true);
        tapToContinueButton.onClick.RemoveAllListeners();
        tapToContinueButton.onClick.AddListener(() =>
        {
            victoryRewardScreen.ShowRewardDragDrop(125, 25);
            tapToContinueButton.gameObject.SetActive(false);
        });
    }
    public void CorrectIngredientOrElemental(string ingredientName) // Khi cho nguyên liệu/ nguyên tố đúng
    {
        // TODO: Hiện feedback visual + audio
        if (floatingTextPrefab != null)
        {
            //Debug.Log("Chạy floating text");
            GameObject objectText = Instantiate(floatingTextPrefab, canvas.transform.position, Quaternion.identity, canvas.transform);
            objectText.GetComponent<FloatingText>().SetText(ingredientName, Color.green);
            Destroy(objectText, 1.5f); // Hủy sau 2 giây
        }
    }

    public void WrongIngredientOrElemental() // khi cho nguyên liệu sai
    {
        health.DecreaseHealth(1);
        // TODO: Hiện feedback sai
        if (floatingTextPrefab != null)
        {
            //Debug.Log("Chạy floating text");
            GameObject objectText = Instantiate(floatingTextPrefab, canvas.transform.position, Quaternion.identity, canvas.transform);
            objectText.GetComponent<FloatingText>().SetText("WRONG", Color.red);
            Destroy(objectText, 1.5f); // Hủy sau 2 giây
        }
    }
    public virtual void PlayInstructionLine() // đoạn voice hướng dẫn khi chơi game
    {
        Debug.Log("Chạy âm thanh giới thiệu hướng dẫn");
    }
    public virtual void StartHandTutorial() // bàn tay hướng dẫn khi vào game
    {

    }


    public void OnDestroy()
    {
        health.OnHealthZero -= EndGame;
       
        countDownTimer.OnTimeUp -= EndGame;
    }
}