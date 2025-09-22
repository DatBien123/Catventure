using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;


[System.Serializable]
public enum ETutorialState
{
    None = 0,
    Showing,
    Hiding
}
public class TutorialIndicator : MonoBehaviour
{
    public ETutorialState TutorialState = ETutorialState.None;
    [Header("References")]
    public GameObject Indicator;
    public TutorialContent TutContentIndicator;

    public Animator IndicatorAnimator;

    public TutorialManager TutorialManager;

    private void Start()
    {
        StartShowTutorials();
    }

    Coroutine C_ShowTutorial;
    public void StartShowTutorials()
    {
        if(C_ShowTutorial != null)StopCoroutine(C_ShowTutorial);
        C_ShowTutorial = StartCoroutine(ShowTutorial());
    }
    IEnumerator ShowTutorial()
    {

        while(TutorialManager.isApplyTutorials)
        {
            if (TutorialState != ETutorialState.Showing)
            {
                if (TutorialManager.currentStep.InteractType == ETutorialType.Tap || TutorialManager.currentStep.InteractType == ETutorialType.TapRandom)
                {
                    StartIndicateTap(TutorialManager.currentStep);
                }
                else if (TutorialManager.currentStep.InteractType == ETutorialType.Drag)
                {
                    StartIndicateDrag(TutorialManager.currentStep);
                }
            }
            yield return null;
        }

    }


    #region [Indicate Tap]
    Coroutine C_IndicateTap;
    public void StartIndicateTap(TutorialStep tutorialStep)
    {
        if(C_IndicateTap != null)StopCoroutine(C_IndicateTap);
        C_IndicateDrag = StartCoroutine(ShowIndicatorTap(tutorialStep));
    }
    IEnumerator ShowIndicatorTap(TutorialStep tutorialStep)
    {
        TutorialState = ETutorialState.Showing;
        TutorialManager.AllowNextStep = false;

        Debug.Log("Go here");
        //Set Up
        if(TutorialManager.currentStep.isShowIndicator) Indicator.gameObject.SetActive(true);
        if (TutorialManager.currentStep.isShowTutContent) TutContentIndicator.gameObject.SetActive(true);

        //Data Setup
        TutContentIndicator.SetupTutorialContent(TutorialManager.currentStep);

        IndicatorAnimator.CrossFadeInFixedTime("Tap", 0.0f);

        Indicator.GetComponent<RectTransform>().anchoredPosition = tutorialStep.TapOffset.PositionOffset;
        TutContentIndicator.GetComponent<RectTransform>().anchoredPosition = tutorialStep.TutContentOffset.PositionOffset;

        while (!TutorialManager.AllowNextStep)
        {
            yield return null;
        }
        Indicator.gameObject.SetActive(false);
        TutContentIndicator.gameObject.SetActive(false);

        TutorialState = ETutorialState.Hiding;
        TutorialManager.OnNextTutorialStep();
    }
    #endregion

    #region [Indicate Drag]
    [Header("Drag Indicator")]
    public float DragDuration = 1.0f; // Thời gian di chuyển giữa hai điểm liên tiếp (tùy chỉnh tốc độ)
    public float LoopDelay = 0.5f;    // Thời gian chờ trước khi lặp lại animation (để người chơi có thời gian làm theo)

    Coroutine C_IndicateDrag;
    public void StartIndicateDrag(TutorialStep tutorialStep)
    {
        if (C_IndicateDrag != null) StopCoroutine(C_IndicateDrag);
        C_IndicateDrag = StartCoroutine(ShowIndicatorDrag(tutorialStep));
    }

    IEnumerator ShowIndicatorDrag(TutorialStep tutorialStep)
    {
        if (tutorialStep.DragOffsets == null || tutorialStep.DragOffsets.Count < 2)
        {
            Debug.LogWarning("Drag sequence needs at least 2 points!");
            yield break;
        }

        TutorialState = ETutorialState.Showing;
        TutorialManager.AllowNextStep = false;

        // Set Up ban đầu
        if (TutorialManager.currentStep.isShowIndicator) Indicator.gameObject.SetActive(true);
        if (TutorialManager.currentStep.isShowTutContent) TutContentIndicator.gameObject.SetActive(true);

        IndicatorAnimator.CrossFadeInFixedTime("Drag", 0.0f);

        TutContentIndicator.GetComponent<RectTransform>().anchoredPosition = tutorialStep.TutContentOffset.PositionOffset;
        RectTransform indicatorRT = Indicator.GetComponent<RectTransform>();

        while (!TutorialManager.AllowNextStep)
        {
            // Di chuyển qua từng đoạn trong sequence
            for (int i = 0; i < tutorialStep.DragOffsets.Count - 1; i++)
            {
                TransformOffset start = tutorialStep.DragOffsets[i];
                TransformOffset end = tutorialStep.DragOffsets[i + 1];

                // Vị trí, rotation, scale ban đầu
                indicatorRT.anchoredPosition = start.PositionOffset;
                indicatorRT.localRotation = Quaternion.Euler(start.RotationOffset);
                indicatorRT.localScale = start.ScaleOffset;

                float elapsedTime = 0f;

                while (elapsedTime < DragDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / DragDuration;

                    // Lerp vị trí
                    indicatorRT.anchoredPosition = Vector2.Lerp(start.PositionOffset, end.PositionOffset, t);

                    // Lerp rotation (nếu cần xoay ngón tay theo hướng)
                    Vector3 startRot = start.RotationOffset;
                    Vector3 endRot = end.RotationOffset;
                    indicatorRT.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, endRot, t));

                    // Lerp scale (nếu cần hiệu ứng scale)
                    indicatorRT.localScale = Vector2.Lerp(start.ScaleOffset, end.ScaleOffset, t);

                    // (Tùy chọn) Kiểm tra input drag của người chơi ở đây (xem phần dưới)
                    CheckPlayerDragInput(); // Gọi để phát hiện nếu người chơi làm theo

                    yield return null;
                }

                // Đảm bảo đến đúng vị trí cuối đoạn
                indicatorRT.anchoredPosition = end.PositionOffset;
                indicatorRT.localRotation = Quaternion.Euler(end.RotationOffset);
                indicatorRT.localScale = end.ScaleOffset;
            }

            // Chờ một chút trước khi lặp lại animation
            yield return new WaitForSeconds(LoopDelay);
        }

        // Hoàn thành step
        Indicator.gameObject.SetActive(false);
        TutContentIndicator.gameObject.SetActive(false);
        TutorialState = ETutorialState.Hiding;
        TutorialManager.OnNextTutorialStep();
    }
    #endregion

    // (Tùy chọn) Hàm để phát hiện input drag của người chơi
    private void CheckPlayerDragInput()
    {
        // Logic đơn giản: Phát hiện nếu có drag (swipe) trên screen
        if (Input.GetMouseButtonDown(0)) // Bắt đầu drag (touch/mouse down)
        {
            // Bắt đầu theo dõi drag
            Vector2 startPos = Input.mousePosition;
            StartCoroutine(DetectDragCompletion(startPos));
        }
    }

    IEnumerator DetectDragCompletion(Vector2 startPos)
    {
        while (Input.GetMouseButton(0)) // Đang drag
        {
            yield return null;
        }

        Vector2 endPos = Input.mousePosition;
        float dragDistance = Vector2.Distance(startPos, endPos);

        // Nếu drag đủ dài (ví dụ: > 100 pixels), coi như hoàn thành
        if (dragDistance > 100f) // Tùy chỉnh ngưỡng
        {
            // (Nâng cao: Kiểm tra nếu drag theo hướng gần giống sequence, nhưng giữ đơn giản)
            TutorialManager.AllowNextStep = true;
        }
    }
}

