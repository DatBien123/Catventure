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
    public TimelineEventDienTu timeLineCutscene; // cutscene của FITB Minigame
    void Start()
    {
        startMinigamePanel.GetComponent<StartMinigamePanel>().Setup("TRẢ KIẾM", null, null, levelData.timeRequired, levelData.reward);

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
        }
    }

    public void ShowRewardUI()
    {
        EndGame(true);
        inputBlocker.SetActive(false);
        victoryRewardScreen.ShowRewardFITB(levelData.reward,3);
    }

    void OnAnswerSelected(bool isCorrect)
    {
        FITBQuestionSO currentQuestion = levelData.questions[currentIndex];
        if (isCorrect)
        {
            currentIndex++;
            correctAnswers++;
            fitbUI.FillBlankWithAnswer(currentQuestion.correctAnswer);
            fitbUI.ShowCorrectFeedback();
            //PlayVideoAfterAnswerQuestion(currentQuestion);
            inputBlocker.SetActive(true);
            DOVirtual.DelayedCall(3f, () =>
            {

                VideoManager.instance.PlayVideo(currentQuestion.correctAnswerVideo, () =>
                {
                    if (!IsGameWin())  // kiểm tra trước khi gọi
                    {
                        VideoManager.instance.videoPanel.SetActive(false);  
                        ShowQuestion(currentIndex);
                    }
                    else
                    {
                        AudioManager.instance.StopAllMusic();
                        Debug.Log("Đã trả lời xong hết hãy hiển thị timeline cutscene ở đây");
                        timeLineCutscene.director.Play();
                        countDownTimer.StopCountDown();
                        //ShowRewardUI();
                    }
                });
            });

 
        }
        else
        {
            inputBlocker.SetActive(true);
            fitbUI.ShowWrongFeedback();
            health.DecreaseHealth(1);
            DOVirtual.DelayedCall(3f, () =>
            {
                if (!IsGameOver())  // kiểm tra trước khi gọi
                {
                    VideoManager.instance.PlayVideo(currentQuestion.wrongAnswerVideo, () =>
                    {
                        VideoManager.instance.videoPanel.SetActive(false);
                        ShowQuestion(currentIndex);
                    });
                }
            });
        }
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
    // Ví dụ mấy hàm kiểm tra này
    bool IsGameOver()
    {
        return health.currentHealth == 0;
    }

    bool IsGameWin()
    {
        return currentIndex >= levelData.questions.Count && countDownTimer.IsEnoughTimeToWin() && health.currentHealth > 0;
    }


}
