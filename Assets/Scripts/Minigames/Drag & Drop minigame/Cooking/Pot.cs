using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;



[System.Serializable]
public struct IngredientOffsetData
{
    public float offsetX;
    public float offsetY;
}
public class Pot : MonoBehaviour, IDropHandler
{
    // danh sách các nguyên liệu cần của món ăn hiện tại
    public CraftingRecipeSO currentRecipe;
    public List<IngredientOffsetData> ingredientOffsetDatas;
    [SerializeField] private List<string> addedIngredients = new List<string>();    
    public List<GameObject> ingredientInPot; // Danh sách những nguyên liệu ta đã add vào nồi 
    public ParticleSystem smokeEffect;
    public ParticleSystem fireEffect;
    public ParticleSystem smokePuffEffect;
    public ParticleSystem starPuffEffect;
    public Tween shakeTween; // dùng để dừng lại sau
    public Action<bool> OnCookingComplete;
    

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
    public void Init(CraftingRecipeSO recipe)
    {
        currentRecipe = recipe;

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;
        CraftingItem ingredient = dropped.GetComponent<CraftingItem>();
        CraftingItemSO data = ingredient.crafttingItemData;
        string ingredientName = ingredient.GetCraftingItem();
        int currentIndex = addedIngredients.Count;
        if(currentIndex < currentRecipe.requiredItems.Count && data == currentRecipe.requiredItems[currentIndex])
        {
            addedIngredients.Add(ingredientName);
            // cập nhật Ingredient Bar
            CookingMinigame.Instance.cookingUI.ingredientBarUI.MarkItemAsCollected(data);
            //Debug.Log("✅ Thêm nguyên liệu đúng: " + ingredientName);
            CookingMinigame.Instance.CorrectIngredientOrElemental(ingredientName);
            dropped.transform.SetParent(this.transform);
            ingredient.rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-14, ingredientOffsetDatas[0].offsetX), ingredientOffsetDatas[0].offsetY);
            ingredientInPot.Add(dropped);
            AudioManager.instance.PlaySFX("Correct Ingredient");
            Destroy(dropped);
        }
        else
        {
            // trả nguyên liệu về đúng chỗ ban đầu
            CookingMinigame.Instance.WrongIngredientOrElemental();
            ingredient.ReturnToOriginalPosition(); // ✅ Trả về
            AudioManager.instance.PlaySFX("Incorrect Ingredient");

        }
        if (addedIngredients.Count == currentRecipe.requiredItems.Count)
        {
            CookingMinigame.Instance.HandleCookingDragAndDrop();
            CookingMinigame.Instance.ShowQTEBar();

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
        float originalX = transform.position.x;
        shakeTween = transform.DOMoveX(originalX + 1f, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); AudioManager.instance.PlaySFX("Cooking Sound");
        if (smokeEffect != null)
        {
            smokeEffect.Play(); // 🎉 Bắt đầu khói bốc lên!
        }
        if (fireEffect != null) { fireEffect.Play();  }
        // Gọi hàm nấu xong sau 3s
        Invoke(nameof(StopParticleEffect), 3f);
        Invoke(nameof(CookingComplete), 6f);
    }
    public void CookingComplete()
    {
        if (smokePuffEffect != null)
            smokePuffEffect.Play();

        if (starPuffEffect != null)
            starPuffEffect.Play();

        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill(); // dừng tween
            transform.rotation = Quaternion.identity; // đưa về vị trí ban đầu
        }
        AudioManager.instance.PlaySFX("New Food"); // optional: âm thanh “bling!”
        Debug.Log("🍲 Món ăn đã hoàn thành!");
        // bỏ hết các nguyên liệu đang trong nồi
        foreach(GameObject obj in ingredientInPot)
        {
            Destroy(obj);
        }
        OnCookingComplete?.Invoke(true);
    }

    // Hiệu ứng khi người chơi chọn đúng Perfect Zone trong QTE
    public void ShowPerfectZoneEffect()
    {
        Debug.Log("Chạy hiệu ứng hoàn hảo");
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.DOShakeScale(0.3f, strength: 1.5f, vibrato: 10, randomness: 90, fadeOut: true);
    }

    // Hiệu ứng khi người chơi chọn Good Zone trong QTE
    public void ShowGoodZoneEffect()
    {
        Debug.Log("Chạy hiệu ứng tốt");
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.DOShakePosition(0.3f, strength: new Vector3(20f, 20f, 0f), vibrato: 10, randomness: 90, fadeOut: true);

    }

}

