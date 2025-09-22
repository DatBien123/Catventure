using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public TransformOffset TapOffset;

    public List<TransformOffset> DragOffsets;

    [Header("Tutorial Content")]

    public bool isShowTutContent;

    public Sprite ImageTutContent;

    [TextArea] public string TutContent;

    public TransformOffset TutContentOffset;

    [Header("Target Button (Optional)")] // Thêm này để reference trực tiếp
    public UnityEngine.UI.Button TargetButton; // Kéo thả button từ Hierarchy vào Editor

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

    private void Awake()
    {
        // Lấy tất cả button từ Hierarchy (bao gồm inactive)
        allButtons.AddRange(FindObjectsOfType<Button>(true));

        // Khởi tạo tutorial
        currentPart = tutorialDatabase.TutorialParts[currentTutorialIndex];
        currentStep = currentPart.TutorialSteps[currentStepIndex];

        // Disable tất cả button trừ target nếu là Tap
        if (isApplyTutorials && currentStep.InteractType == ETutorialType.Tap)
        {
            DisableAllButtonsExceptTarget(currentStep.TargetButton, currentStep.stepName);
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
        if (targetButton != null)
        {
            targetButton.interactable = true;
        }
        else if (!string.IsNullOrEmpty(targetButtonName))
        {
            Button target = allButtons.Find(b => b.name == targetButtonName);
            if (target != null)
            {
                target.interactable = true;
            }
            else
            {
                Debug.LogWarning($"Button '{targetButtonName}' not found in Hierarchy!");
            }
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

    public void OnNextTutorialPart()
    {
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
        }
    }

    public void OnNextTutorialStep()
    {
        if (currentStepIndex < currentPart.TutorialSteps.Count - 1)
        {
            currentStepIndex++;
            currentStep = currentPart.TutorialSteps[currentStepIndex];

            if(currentStep.InteractType == ETutorialType.TapRandom)
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
