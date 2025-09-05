using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
public class FitbMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private VictoryRewardScreen victoryRewardScreen;
    // khi bắt đầu game thì sẽ có một danh sách các câu hỏi cho 1 lần chơi
    [SerializeField] private FitbLevelData levelData;
    [SerializeField] private FitbUI fitbUI; // Fill in the blank User interface 
    [SerializeField] private int currentIndex = 0; // index tương ứng với câu hỏi hiện tại trong questions
    [SerializeField] private HealthSystem healthSystem; // hệ thống máu để xử lý game over
    [SerializeField] private GameObject inputBlockerPanel;
    public CountDownTimerSystem timer;
    [SerializeField] private int correctAnswers = 0;

    // UI
    public GameObject startMinigamePanel;
    public GameObject userInterface;
    void Awake()
    {

    }
    void Start()
    {
        startMinigamePanel.GetComponent<StartMinigamePanel>().Setup("ĐIỀN TỪ",null,null, levelData.timeRequired, levelData.reward);

    }
    private void OnEnable()
    {
        // Ta sẽ đăng ký sự kiện cho các hàm UIEventSystem ở đây
        UIEventSystem.Register("Start Game", StartGame);

    }
    private void OnDisable()
    {
        UIEventSystem.Unregister("Start Game", StartGame);

    }
    public void StartGame()
    {
        timer.SetTimeStart(levelData.timeRequired);
        startMinigamePanel.SetActive(false);
        userInterface.SetActive(true);
        healthSystem.gameObject.SetActive(true);
        timer.StartCountDown();
        correctAnswers = 0;
        AudioManager.instance.PlaySFX("Starting Minigame");
        AudioManager.instance.PlayMusic("FITB Background Music");
        currentIndex = 0;
        ShowQuestion(currentIndex);
        timer.OnTimeUp += EndGame;
        healthSystem.OnHealthZero += EndGame;
    }

    public void EndGame(bool success)
    {
        // cứ end game là dừng biến đếm thời gian lại không cần biết thắng hay thua
        timer.StopCountDown();
        if (success)
        {
            victoryRewardScreen.ShowRewardFITB(levelData.reward);
        }
        else
        {
            Debug.Log("Người chơi đã thua");
            // gọi game over
            GameOver();
        }
    }


    public void GameOver()
    {
        gameOverScreen.Setup();
    }
    // Hàm này là để hiển thị ra câu hỏi, các đáp án và hình minh họa của question hiện tại trong danh sách Questions ở trên
    private void ShowQuestion(int currentIndex)
    {
        if (currentIndex < levelData.questions.Count)
        {
            inputBlockerPanel.SetActive(false);
            fitbUI.UpdateQuestionCounter(currentIndex + 1, levelData.questions.Count);
            fitbUI.DisplayQuestion(levelData.questions[currentIndex], OnAnswerSelected); // ta truyền vào hàm OnAnswerSelected để kiểm tra đáp án khi player ấn vào có đúng không
        }
        else
        {
            if (timer.IsEnoughTimeToWin())
            {
                EndGame(true); // ✅ Khi hết câu hỏi
            }
        }
    }
    void OnAnswerSelected(bool isCorrect)
    {
        inputBlockerPanel.SetActive(true);
        FITBQuestionSO currentQuestion = levelData.questions[currentIndex];
        currentIndex++;
        if (isCorrect)
        {
            correctAnswers++;
            fitbUI.ShowCorrectFeedback();
            //PlayVideoAfterAnswerQuestion(currentQuestion);
            inputBlockerPanel.SetActive(true);
            DOVirtual.DelayedCall(3f, () => ShowQuestion(currentIndex));
        }
        else
        {
            fitbUI.ShowWrongFeedback();
            healthSystem.DecreaseHealth(1);
            DOVirtual.DelayedCall(3f, () => ShowQuestion(currentIndex));

        }

    }
    private void OnDestroy()
    {
        timer.OnTimeUp -= EndGame;
    }

}
