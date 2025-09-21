using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
public class FitbMinigame : BaseMinigame
{
    // khi bắt đầu game thì sẽ có một danh sách các câu hỏi cho 1 lần chơi
    [SerializeField] private FitbLevelData levelData;
    [SerializeField] private FitbUI fitbUI; // Fill in the blank User interface 
    [SerializeField] private int currentIndex = 0; // index tương ứng với câu hỏi hiện tại trong questions
    [SerializeField] private int correctAnswers = 0;

    // UI
    public GameObject startMinigamePanel;
    public GameObject userInterface;

    void Start()
    {
        startMinigamePanel.GetComponent<StartMinigamePanel>().Setup("ĐIỀN TỪ", null, null, levelData.timeRequired, levelData.reward);

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        UIEventSystem.Register("Start Game", StartGame);

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        UIEventSystem.Unregister("Start Game", StartGame);
    }
    public override void StartGame()
    {
        base.StartGame();
        countDownTimer.SetTimeStart(levelData.timeRequired);
        startMinigamePanel.SetActive(false);
        userInterface.SetActive(true);
        health.gameObject.SetActive(true);
        countDownTimer.StartCountDown();
        correctAnswers = 0;
        AudioManager.instance.PlaySFX("Starting Minigame");
        AudioManager.instance.PlayMusic("FITB Background Music");
        currentIndex = 0;
        ShowQuestion(currentIndex);
    }

    public override void EndGame(bool success)
    {
    base.EndGame(success);
    //DOTween.KillAll(); // ✅ hủy toàn bộ delayed call khi end game

    }


    public override void GameOver()
    {
        base.GameOver();
    }
    // Hàm này là để hiển thị ra câu hỏi, các đáp án và hình minh họa của question hiện tại trong danh sách Questions ở trên
    private void ShowQuestion(int currentIndex)
    {
        if (currentIndex < levelData.questions.Count)
        {
            inputBlocker.SetActive(false);
            fitbUI.UpdateQuestionCounter(currentIndex + 1, levelData.questions.Count);
            fitbUI.DisplayQuestion(levelData.questions[currentIndex], OnAnswerSelected); // ta truyền vào hàm OnAnswerSelected để kiểm tra đáp án khi player ấn vào có đúng không
        }
        else
        {
            if (countDownTimer.IsEnoughTimeToWin() && health.currentHealth > 0)
            {
                ShowRewardUI();
            }
        }
    }

    private void ShowRewardUI()
    {
        inputBlocker.SetActive(false);
        victoryRewardScreen.ShowRewardFITB(levelData.reward,3);
    }

    void OnAnswerSelected(bool isCorrect)
    {
        FITBQuestionSO currentQuestion = levelData.questions[currentIndex];
        currentIndex++;
        if (isCorrect)
        {
            correctAnswers++;
            fitbUI.ShowCorrectFeedback();
            //PlayVideoAfterAnswerQuestion(currentQuestion);
            inputBlocker.SetActive(true);
            DOVirtual.DelayedCall(3f, () => ShowQuestion(currentIndex));
        }
        else
        {
            fitbUI.ShowWrongFeedback();
            health.DecreaseHealth(1);
            DOVirtual.DelayedCall(3f, () => ShowQuestion(currentIndex));

        }

    }


}
