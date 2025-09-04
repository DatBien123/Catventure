using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

// Manager quản lý các User Interface của riêng Minigame chọn từ để hoàn thành câu
public class FitbUI : MonoBehaviour
{
    [SerializeField] private Text sentenceText;
    [SerializeField] private Image illustration;
    [SerializeField] private List<AnswerButton> answerButtons;
    [SerializeField] private Text questionCounterText;

    private System.Action<bool> callback;
    // Display Question hiên thị câu hỏi , hình minh họa và các đáp án cho 3 nút bấm
    public void DisplayQuestion(FITBQuestionSO question, System.Action<bool> onAnsweredCheck)     // onAswered là một event ta gán vào biến callback sẽ chạy nó khi player chọn 1 đáp án
    {
        callback = onAnsweredCheck; // gán event onAswered cho callback này thì khi callback Invoke thì hàm onAnswered cũng được gọi
        SetQuestionData(question);

        // xóa các đáp án cũ nếu có
        foreach (AnswerButton button in answerButtons)
        {
            button.answerText = "";
        }
        SetupAnswerButtons(question);

    }
    public void ShowWrongFeedback()
    {
        Debug.Log("❌ Sai rồi, thử lại nhé!");
        // Optional: rung nút, đỏ màu, v.v.
        AudioManager.instance.PlaySFX("Wrong Answer");
    }
    public void ShowCorrectFeedback()
    {
        AudioManager.instance.PlaySFX("Correct Answer");
    }
    private void SetQuestionData(FITBQuestionSO question)
    {
        sentenceText.text = question.sentenceWithBlank; // gán text cho câu hỏi trên màn hình
        illustration.sprite = question.illustration; // gán ảnh minh họa của câu hỏi này
    }
    private void SetupAnswerButtons(FITBQuestionSO question)
    {
        int buttonCount = Mathf.Min(answerButtons.Count, question.answers.Count);

        for (int i = 0; i < buttonCount; i++)
        {
            var button = answerButtons[i];
            string answer = question.answers[i];

            button.Setup(answer, (string selectedAnswer) =>
            {
                bool isCorrect = selectedAnswer == question.correctAnswer;

                foreach (var btn in answerButtons)
                {
                    if (btn == button)
                    {
                        if (isCorrect) btn.effect.ShowCorrectEffect();
                        else btn.effect.ShowWrongEffect();
                    }
                    else
                    {
                        if (btn.answerText == question.correctAnswer)
                            btn.effect.ShowOnlySmallCheck();
                        else
                            btn.effect.FadeOut();
                    }
                }

                callback?.Invoke(isCorrect);

                DOVirtual.DelayedCall(0.5f, () =>
                {
                    if (isCorrect)
                        AudioManager.instance.PlayPronunciation(question.pronunciation);
                });
            });

            Vector3 originalScale = button.transform.localScale;
            button.transform.localScale = Vector3.zero;
            button.transform.DOScale(originalScale, 0.5f)
                  .SetEase(Ease.OutBack)
                  .SetDelay(i * 0.1f);
        }

        // Disable các nút thừa nếu có
        for (int i = buttonCount; i < answerButtons.Count; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
    }
    public void UpdateQuestionCounter(int currentQuestion, int totalQuestions)
    {
        questionCounterText.text = $"{currentQuestion}/{totalQuestions}";
    }
}
