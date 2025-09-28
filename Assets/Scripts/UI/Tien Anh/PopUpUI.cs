using System;
using UnityEngine;
using UnityEngine.UI;
public class PopUpUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Button okButton;
    [SerializeField] private GameObject buttonGroupConfirm;
    [SerializeField] private GameObject buttonGroupOk;
    [SerializeField] private TutorialManager tutorialManager;

    private Action onYes;
    private Action onNo;
    private Action onOk;
    // Mở popup kiểu Confirm (Yes/No)
    public void ShowConfirm(string title, string message, Action yesCallback, Action noCallback = null)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        messageText.text = message;

        buttonGroupConfirm.SetActive(true);
        buttonGroupOk.SetActive(false);

        onYes = yesCallback;
        onNo = noCallback;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => {
            if (tutorialManager != null)
            {
                if (tutorialManager.currentPart.TutorialName == "Cooking Tutorial" && tutorialManager.currentStep.stepName == "Xác nhận nấu")
                {
                    tutorialManager.ApplyNextStep("Xác nhận nấu");
                }
            }
            onYes?.Invoke();
            Hide();
        });

        noButton.onClick.AddListener(() => {
            onNo?.Invoke();
            Hide();
        });
    }

    // Mở popup kiểu Notify (OK)
    public void ShowNotify(string title, string message, Action okCallback = null)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        messageText.text = message;

        buttonGroupConfirm.SetActive(false);
        buttonGroupOk.SetActive(true);

        onOk = okCallback;

        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() => {
            onOk?.Invoke();
            Hide();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
