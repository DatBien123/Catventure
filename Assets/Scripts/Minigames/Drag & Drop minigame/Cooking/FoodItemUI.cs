using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
public class FoodItemUI : MonoBehaviour
{
    public Image icon; // ảnh của món ăn
    public Text nameText; // tên của món ăn
    public DishSO foodData; // dữ liệu của món ăn

    public Outline outline;
    private Tween outlineTween;

    private bool isSelected = false;
    private Action<DishSO, FoodItemUI> OnSelected;
    // Hàm Setup khi spawn ra
    public void Setup(DishSO data, Action<DishSO, FoodItemUI> callback)
    {
        foodData = data;    
        icon.sprite = foodData.icon;   
        nameText.text = foodData.dishName;
        OnSelected = callback;  

    }
    // Khi ta bấm vào Item trong Food Selection
    public void OnClick()
    {
        // Gọi sang Food Selection UI để cập nhật
        // FoodSelectionUI. show thông tin gì đó truyền food data vào
        OnSelected?.Invoke(foodData,this);
    }

    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        if (isSelected)
        {
            outline.enabled = true;

            // Đặt alpha về 0 ngay khi bắt đầu
            Color c = outline.effectColor;
            c.a = 0f;
            outline.effectColor = c;

            // Bắt đầu tween từ alpha = 0 lên 0.5 rồi nhấp nháy qua lại
            outlineTween = DOTween.ToAlpha(
                () => outline.effectColor,
                x => outline.effectColor = x,
                0.5f,   // alpha mục tiêu (sáng nhất)
                0.6f    // thời gian một chiều
            )
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        }
        else
        {
            outlineTween?.Kill();
            outline.enabled = false;
        }
    }
    private void OnDestroy()
    {
        outlineTween?.Kill();
        outlineTween = null;
    }
}
