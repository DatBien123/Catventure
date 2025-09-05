using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
public class AnswerButton : MonoBehaviour
{
    // Đây là class về nút bấm tương ứng với 1 câu trả lời trên màn hinh của minigame điền từ hoàn thành 1 câu
    public string answerText;
    [SerializeField] private Text answerLabel;
    [SerializeField] private Button button;
    public AnswerButtonEffect effect;
    private System.Action<string> onClickCallback; // biến nhận sự kiện khi nút này được bấm

    private void Awake()
    {
        effect = GetComponent<AnswerButtonEffect>();    
    }
    // setup dữ liệu cho button hiển thị

    public void Setup(string text, Action<string> callback)
    {
        answerText = text;
        answerLabel.text = text;
        onClickCallback = callback;
        // Reset effect
        effect.ResetEffects();
        GetComponent<Button>().interactable = true;
    }
    // hàm nhận sự kiện khi bấm vào button này
    public void OnClick()
    {
        onClickCallback?.Invoke(answerText); // ta sẽ invoke để báo đến cho FitbManager kiểm tra xem đáp án của button này có đúng không. Vì kiểm tra logic đúng/sai của minigame này không thể thực hiện trong này
        effect.ClickEffect();

    }

    public Text GetButtonLabel() { return answerLabel; }    
}
