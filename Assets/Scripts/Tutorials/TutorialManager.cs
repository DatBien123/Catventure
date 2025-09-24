using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import DOTween

[System.Serializable]
public enum ETutorialType
{
    Tap,
    Drag,
    TapRandom
}
[System.Serializable]
public struct TutorialStep
{
    public string stepName;

    public ETutorialType InteractType;

    [Header("Indicator")]

    public bool isShowIndicator;

    public GameObject Indicator;

    public TransformOffset TapOffset;

    public List<TransformOffset> DragOffsets;

    [Header("Tutorial Content")]

    public bool isShowTutContent;

    //public GameObject TutContentIndicator;

    public Sprite ImageTutContent;

    [TextArea] public string TutContent;

    public TransformOffset TutContentOffset;

    public Vector2 SizeOffset;

    [Header("Target Button (Optional)")] // Thêm này để reference trực tiếp
    public UnityEngine.UI.Button TargetButton; // Kéo thả button từ Hierarchy vào Editor
    public bool isHightlightButton;

    public bool isStepCompleted;

}
[System.Serializable]
public struct TransformOffset
{
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
    public Vector3 ScaleOffset;
}

[System.Serializable]
public struct TutorialPart
{
    public string TutorialName;
    public List<TutorialStep> TutorialSteps;
    public bool isDisableAllButtons;
    public bool isPartCompleted;
}

[System.Serializable]
public struct TutorialData
{
    public List<TutorialPart> TutorialParts;
}


public class TutorialManager : MonoBehaviour
{
    public bool isApplyTutorials = false;

    public TutorialData tutorialDatabase;

    public TutorialPart currentPart;

    public int currentTutorialIndex = 0;

    public TutorialStep currentStep;

    public int currentStepIndex = 0;

    public bool AllowNextStep = false;

    private List<Button> allButtons = new List<Button>(); // Danh sách tất cả button trong Hierarchy

    public TutorialBackground TutorialBackground;

    public BackgroundTouchHandle BackgroundTouchHandle;

    [Header("Button Highlight Animation")]
    public float HighlightPulseDuration = 1.0f; // Thời gian cho một chu kỳ pulse
    public Vector3 HighlightScaleMultiplier = new Vector3(1.1f, 1.1f, 1.1f); // Scale lên bao nhiêu (ví dụ: 1.1 lần)

    private Tween currentHighlightTween; // Tween hiện tại để highlight button

    private void Awake()
    {
        // Lấy tất cả button từ Hierarchy (bao gồm inactive)
        allButtons.AddRange(FindObjectsOfType<Button>(true));

        // Khởi tạo tutorial
        currentPart = tutorialDatabase.TutorialParts[currentTutorialIndex];
        currentStep = currentPart.TutorialSteps[currentStepIndex];

        //Data reader
        TutorialSaveSystem.Load(this);

        var tutPart = tutorialDatabase.TutorialParts.Find(tutPart => tutPart.TutorialName == currentPart.TutorialName);
        currentPart.isPartCompleted = tutPart.isPartCompleted;
        //Data reader

        if (!currentPart.isPartCompleted)
        {
            if (currentPart.isDisableAllButtons) DisableAllButtons();

            if (currentStep.InteractType == ETutorialType.TapRandom)
            {
                TutorialBackground.SetCanReceiveTouch(true);
            }
            else
            {
                TutorialBackground.SetCanReceiveTouch(false);
            }
            // Disable tất cả button trừ target nếu là Tap
            if (isApplyTutorials && currentStep.InteractType == ETutorialType.Tap)
            {
                DisableAllButtonsExceptTarget(currentStep.TargetButton, currentStep.stepName);
            }

            isApplyTutorials = true;
        }
        else
        {
            isApplyTutorials = false;
        }

    }

    // Hàm disable tất cả button trừ target (dựa trên reference hoặc tên)
    public void DisableAllButtonsExceptTarget(Button targetButton, string targetButtonName)
    {
        foreach (Button btn in allButtons)
        {
            btn.interactable = false; // Disable tất cả trước
        }

        // Enable button mục tiêu
        Button enabledButton = null;
        if (targetButton != null)
        {
            targetButton.interactable = true;
            enabledButton = targetButton;
        }
        else if (!string.IsNullOrEmpty(targetButtonName))
        {
            Button target = allButtons.Find(b => b.name == targetButtonName);
            if (target != null)
            {
                target.interactable = true;
                enabledButton = target;
            }
            else
            {
                Debug.LogWarning($"Button '{targetButtonName}' not found in Hierarchy!");
            }
        }

        // Áp dụng animation highlight cho button được enable
        if (enabledButton != null && currentStep.isHightlightButton)
        {
            StartButtonHighlightAnimation(enabledButton);
        }
    }

    // Enable lại tất cả button
    public void EnableAllButtons()
    {
        foreach (Button btn in allButtons)
        {
            btn.interactable = true;
        }
    }
    public void DisableAllButtons()
    {
        foreach (Button btn in allButtons)
        {
            btn.interactable = false;
        }
    }
    // Hàm bắt đầu animation highlight cho button (sử dụng DOTween để pulse scale)
    private void StartButtonHighlightAnimation(Button targetButton)
    {
        // Kill tween cũ nếu có
        if (currentHighlightTween != null)
        {
            currentHighlightTween.Kill();
        }

        // Lấy RectTransform của button
        RectTransform buttonRect = targetButton.GetComponent<RectTransform>();

        // Tạo tween pulse scale: từ scale hiện tại lên multiplier rồi về hiện tại, lặp vô hạn
        currentHighlightTween = buttonRect.DOScale(1.2f, HighlightPulseDuration / 2f)
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn, yoyo (lên xuống)
            .SetEase(Ease.InOutSine); // Ease mượt mà
    }

    // Hàm dừng animation highlight
    private void StopButtonHighlightAnimation()
    {
        if (currentHighlightTween != null && currentStep.isHightlightButton)
        {
            currentStep.TargetButton.GetComponent<RectTransform>().localScale = Vector3.one;
            currentHighlightTween.Kill();
            currentHighlightTween = null;
        }
    }

    public void OnNextTutorialPart()
    {
        // Dừng animation highlight trước khi chuyển part
        StopButtonHighlightAnimation();


        if (currentTutorialIndex < tutorialDatabase.TutorialParts.Count - 1)
        {
            currentTutorialIndex++;
            currentPart = tutorialDatabase.TutorialParts[currentTutorialIndex];
            currentStepIndex = 0;
            currentStep = currentPart.TutorialSteps[currentStepIndex];

            if (currentStep.InteractType == ETutorialType.TapRandom)
            {
                TutorialBackground.SetCanReceiveTouch(true);
            }
            else
            {
                TutorialBackground.SetCanReceiveTouch(false);
            }

            // Xử lý button cho step mới
            if (isApplyTutorials && (currentStep.InteractType == ETutorialType.Tap || currentStep.InteractType == ETutorialType.TapRandom))
            {
                DisableAllButtonsExceptTarget(currentStep.TargetButton, currentStep.stepName);
            }
            else
            {
                EnableAllButtons(); // Enable cho Drag hoặc loại khác
            }
        }
        else
        {
            // Tutorial hoàn thành, enable lại tất cả
            EnableAllButtons();
            isApplyTutorials = false; // (Tùy chọn) Tắt tutorial
            TutorialBackground.SetCanReceiveTouch(false);
        }
    }

    public void OnNextTutorialStep()
    {
        // Dừng animation highlight trước khi chuyển step
        StopButtonHighlightAnimation();


        if (currentStepIndex < currentPart.TutorialSteps.Count - 1)
        {
            currentStepIndex++;
            currentStep = currentPart.TutorialSteps[currentStepIndex];

            if (currentStep.InteractType == ETutorialType.TapRandom)
            {
                TutorialBackground.SetCanReceiveTouch(true);
            }
            else
            {
                TutorialBackground.SetCanReceiveTouch(false);
            }

            // Xử lý button cho step mới
            if (isApplyTutorials && (currentStep.InteractType == ETutorialType.Tap || currentStep.InteractType == ETutorialType.TapRandom))
            {
                DisableAllButtonsExceptTarget(currentStep.TargetButton, currentStep.stepName);
            }
            else
            {
                EnableAllButtons(); // Enable cho Drag hoặc loại khác
                if (currentPart.isDisableAllButtons) DisableAllButtons();
            }
        }
        else
        {
            currentPart.isPartCompleted = true;
            TutorialSaveSystem.Save(this, currentPart); // Lưu khi hoàn thành part
            OnNextTutorialPart();
        }
    }

    public void ApplyNextStep(string buttonName)
    {
        if (currentStep.stepName == buttonName)
        {
            AllowNextStep = true;
        }
    }

    // (Tùy chọn) Reset tutorial về đầu
    public void ResetTutorial()
    {
        // Dừng animation trước khi reset
        StopButtonHighlightAnimation();

        currentTutorialIndex = 0;
        currentStepIndex = 0;
        currentPart = tutorialDatabase.TutorialParts[currentTutorialIndex];
        currentStep = currentPart.TutorialSteps[currentStepIndex];
        if (isApplyTutorials && currentStep.InteractType == ETutorialType.Tap)
        {
            DisableAllButtonsExceptTarget(currentStep.TargetButton, currentStep.stepName);
        }
        else
        {
            EnableAllButtons();
        }
    }
}