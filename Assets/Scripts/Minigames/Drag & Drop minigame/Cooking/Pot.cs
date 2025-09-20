using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;
using UnityEditor.Experimental.GraphView;



[System.Serializable]
public struct IngredientOffsetData
{
    public float offsetX;
    public float offsetY;
}
public class Pot : MonoBehaviour, IDropHandler, IDropIngredientTarget
{
    // danh sách các nguyên liệu cần của món ăn hiện tại
    public CookingStepSO currentStep;
    public List<IngredientOffsetData> ingredientOffsetDatas;
    [SerializeField] private List<string> addedIngredients = new List<string>();
    public GameObject ingredientsInPot;
    public List<GameObject> ingredientInPot; // Danh sách những nguyên liệu ta đã add vào nồi 
    public Image water; // màu nước hiện tại trong nồi
    [SerializeField] private Color targetColor;
    public ParticleSystem smokeEffect;
    public ParticleSystem fireEffect;
    public ParticleSystem smokePuffEffect;
    public ParticleSystem starPuffEffect;
    public Tween shakeTween; // dùng để dừng lại sau

    public void OnEnable()
    {
        QTEBar.onPerfectZone += ShowPerfectZoneEffect;
        QTEBar.onGoodZone += ShowGoodZoneEffect;    
    }
    public void OnDisable()
    {
        QTEBar.onPerfectZone -= ShowPerfectZoneEffect;
        QTEBar.onGoodZone -= ShowGoodZoneEffect;
    }
    public void Init(CookingStepSO step)
    {
        currentStep = step;

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;
        Ingredient ingredient = dropped.GetComponent<Ingredient>();
        IngredientSO data = ingredient.ingredientData;
        string ingredientName = ingredient.GetCraftingItem();
        int currentIndex = addedIngredients.Count;
        if(currentIndex < currentStep.requiredIngredients.Count() && data == currentStep.requiredIngredients[currentIndex])
        {
            addedIngredients.Add(ingredientName);
            // cập nhật Ingredient Bar
            BoilingStirringUI.Instance.ingredientBarUI.MarkItemAsCollected(data);
            BoilingStirringStep.Instance.ShowCorrectEffect(ingredientName);
            dropped.transform.SetParent(ingredientsInPot.transform);
            Vector2 randomPos = new Vector2(UnityEngine.Random.Range(-60f, 60f), UnityEngine.Random.Range(-50f, 0f));
            Vector2 targetPos = new Vector2(randomPos.x, randomPos.y + 50f);
            ingredient.rectTransform.anchoredPosition = targetPos;

            Sequence seq = DOTween.Sequence();
            seq.Append(ingredient.rectTransform.DOAnchorPos(new Vector2(randomPos.x, -50f), 0.2f).SetEase(Ease.InQuad));
            seq.Append(ingredient.rectTransform.DOAnchorPos(randomPos, 0.3f).SetEase(Ease.OutBack));

            seq.AppendCallback(() =>
{
    // hiệu ứng trôi nổi
    ingredient.rectTransform
        .DOAnchorPos(randomPos + new Vector2(0, 10f), 2f) // nhấp nhô lên xuống 10px
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo);

    // xoay nhẹ cho tự nhiên
    ingredient.rectTransform
        .DORotate(new Vector3(0, 0, 5f), 3f, RotateMode.FastBeyond360)
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo);
});

            ingredientInPot.Add(dropped);
            AudioManager.instance.PlaySFX("Correct Ingredient");


        }
        else
        {
            // trả nguyên liệu về đúng chỗ ban đầu
            BoilingStirringStep.Instance.ShowIncorrectEffect();
            ingredient.ReturnToOriginalPosition(); // ✅ Trả về
            AudioManager.instance.PlaySFX("Incorrect Ingredient");

        }
        if (addedIngredients.Count == currentStep.requiredIngredients.Count())
        {
            BoilingStirringStep.Instance.HandleCookingDragAndDrop();
            BoilingStirringStep.Instance.ShowQTEBar();

        }
    }
    public void StopParticleEffect()
    {
        smokeEffect.Stop();
        fireEffect.Stop();
    }
    // Bắt đầu nấu ăn là sẽ chạy các animation của nồi, effect các thứ

    public void Cooking()
    {
        AudioManager.instance.PlaySFX("Boiling Sound");
        //Debug.Log("Nấu");
        float originalX = transform.position.x;
        shakeTween = transform.DOMoveX(originalX + 1f, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        //AudioManager.instance.PlaySFX("Cooking Sound");
        if (smokeEffect != null)
        {
            smokeEffect.Play(); // 🎉 Bắt đầu khói bốc lên!
        }
        if (fireEffect != null) { fireEffect.Play();  }
        // Gọi hàm nấu xong sau 3s
        //Invoke(nameof(StopParticleEffect), 3f);
        //Invoke(nameof(CookingComplete), 6f);
    }

    public void CookingComplete()
    {
        AudioManager.instance.StopSFX("Boiling Sound");
        StopParticleEffect();
        //if (smokePuffEffect != null)
        //    smokePuffEffect.Play();

        //if (starPuffEffect != null)
        //    starPuffEffect.Play();
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill(); // dừng tween
            transform.rotation = Quaternion.identity; // đưa về vị trí ban đầu
        }
        //AudioManager.instance.PlaySFX("New Food"); // optional: âm thanh “bling!”
        // bỏ hết các nguyên liệu đang trong nồi
        foreach(GameObject obj in ingredientInPot)
        {
            Destroy(obj);
        }

    }

    // Hiệu ứng khi người chơi chọn đúng Perfect Zone trong QTE
    public void ShowPerfectZoneEffect()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.DOShakeScale(0.3f, strength: 0.5f, vibrato: 10, randomness: 90, fadeOut: true);
        float t = BoilingStirringStep.Instance.quickTimeEventBar.cookingProgress.slider.value / BoilingStirringStep.Instance.quickTimeEventBar.cookingProgress.slider.maxValue;
        water.color = Color.Lerp(water.color, targetColor, t);
    }

    // Hiệu ứng khi người chơi chọn Good Zone trong QTE
    public void ShowGoodZoneEffect()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.DOShakePosition(0.3f, strength: new Vector3(20f, 20f, 0f), vibrato: 10, randomness: 90, fadeOut: true);
        float t = BoilingStirringStep.Instance.quickTimeEventBar.cookingProgress.slider.value / BoilingStirringStep.Instance.quickTimeEventBar.cookingProgress.slider.maxValue;
        water.color = Color.Lerp(water.color, targetColor, t);
    }

    public void AcceptIngredient(Ingredient ingredient)
    {
        //Debug.Log("Thả vào nồi");
    }
}

