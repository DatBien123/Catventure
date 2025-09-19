using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class ArrowSwipeController : MonoBehaviour
{
    [Header("Mũi tên (gán đúng thứ tự từ trên xuống)")]
    public List<Image> arrows;

    [Header("Sprites")]
    public Sprite arrowWhiteSprite;
    public Sprite arrowGreenSprite;

    [Header("Dao")]
    public RectTransform kitchenKnife;

    private bool isSwiping = false;
    private int currentArrowIndex = 0; // mũi tên hiện tại cần đi qua

    public static Action onCuttedIngredient;

    void Start()
    {
        ResetAllArrows();
    }

    void Update()
    {
        if (!isSwiping) return;

        DetectArrowUnderKnife(kitchenKnife.position);
    }

    void DetectArrowUnderKnife(Vector2 knifePosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = knifePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Image hitArrow = result.gameObject.GetComponent<Image>();
            // chỉ check mũi tên đúng thứ tự hiện tại
            if (hitArrow != null && hitArrow == arrows[currentArrowIndex])
            {
                hitArrow.sprite = arrowGreenSprite;
                currentArrowIndex++;

                // đi hết tất cả mũi tên
                if (currentArrowIndex >= arrows.Count)
                {
                    OnAllArrowsSwiped();
                }
                return;
            }
        }
    }

    public void OnKnifeDragStart()
    {
        isSwiping = true;
        ResetAllArrows();
    }

    public void OnKnifeDragEnd()
    {
        isSwiping = false;
        ResetAllArrows();
    }

    void ResetAllArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.sprite = arrowWhiteSprite;
        }
        currentArrowIndex = 0;
    }

    void OnAllArrowsSwiped()
    {
        Debug.Log("✅ Người chơi đã vuốt qua tất cả các mũi tên!");
        onCuttedIngredient?.Invoke();

        // reset lại để cho phép cắt tiếp ngay (liên tục không cần thả dao)
        ResetAllArrows();
        isSwiping = true;
    }
}
