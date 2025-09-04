using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;



[System.Serializable]
public struct ElementalOffsetData
{
    public float offsetX;
    public float offsetY;
}
public class Smithy : MonoBehaviour, IDropHandler
{
    // danh sách các nguyên liệu cần của món ăn hiện tại
    public CraftingRecipeSO currentRecipe;
    public List<ElementalOffsetData> elementalOffsetDatas;
    [SerializeField] private List<string> addedElementals = new List<string>();
    public List<GameObject> elementalInAnvil; // Danh sách những nguyên liệu ta đã add vào nồi 
    public ParticleSystem smokeEffect;
    public ParticleSystem fireEffect;
    public ParticleSystem smokePuffEffect;
    public ParticleSystem starPuffEffect;
    public Tween shakeTween; // dùng để dừng lại sau
    public Tween pulseTween;
    public Action<bool> OnForgingComplete;


    public void Start()
    {

    }
    public void Init(CraftingRecipeSO recipe)
    {
        currentRecipe = recipe;

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;
        CraftingItem elemental = dropped.GetComponent<CraftingItem>();
        CraftingItemSO data = elemental.crafttingItemData;
        string elementalName = elemental.GetCraftingItem();
        int currentIndex = addedElementals.Count;
        if (currentIndex < currentRecipe.requiredItems.Count && data == currentRecipe.requiredItems[currentIndex])
        {
            addedElementals.Add(elementalName);
            // cập nhật Elemental Bar
            ForgingSwordMinigame.Instance.forgingSwordUI.elementalBarUI.MarkItemAsCollected(data);
            ForgingSwordMinigame.Instance.CorrectIngredientOrElemental(elementalName);
            dropped.transform.SetParent(this.transform);
            elemental.rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(-14, elementalOffsetDatas[0].offsetX), elementalOffsetDatas[0].offsetY);
            elementalInAnvil.Add(dropped);
            AudioManager.instance.PlaySFX("Correct Elemental");
            Destroy(dropped);
        }
        else
        {
            // trả nguyên liệu về đúng chỗ ban đầu
            ForgingSwordMinigame.Instance.WrongIngredientOrElemental();
            elemental.ReturnToOriginalPosition(); // ✅ Trả về
            AudioManager.instance.PlaySFX("Incorrect Elemental");

        }
        if (addedElementals.Count == currentRecipe.requiredItems.Count)
        {
            ForgingSwordMinigame.Instance.HandleForgingSword();
            Debug.Log("Đã đủ nguyên tố bắt đầu rèn liếm");
            AudioManager.instance.PlaySFX("Blacksmith Sound");
            // Rung qua lại theo trục X
            shakeTween = transform.DOMoveX(transform.position.x + 0.1f, 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
            // Phồng lên – thu nhỏ
            pulseTween = transform.DOScale(new Vector3(5.6f, 5.6f, 5.6f), 0.2f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
            if (smokeEffect != null)
            {
                smokeEffect.Play(); // 🎉 Bắt đầu khói bốc lên!
            }
            if (fireEffect != null) { fireEffect.Play(); }
            // Gọi hàm nấu xong sau 3s
            Invoke(nameof(StopParticleEffect), 4f);
            Invoke(nameof(ForgingComplete), 8f);
        }
    }
    public void StopParticleEffect()
    {
        smokeEffect.Stop();
        fireEffect.Stop();
    }
    public void ForgingComplete()
    {
        if (smokePuffEffect != null)
            smokePuffEffect.Play();

        if (starPuffEffect != null)
            starPuffEffect.Play();

        if (shakeTween != null && shakeTween.IsActive() && pulseTween != null && pulseTween.IsActive())
        {
            shakeTween.Kill(); // dừng tween
            pulseTween.Kill();

            transform.rotation = Quaternion.identity; // đưa về vị trí ban đầu
        }
        AudioManager.instance.PlaySFX("New Food"); // optional: âm thanh “bling!”
        Debug.Log("🍲 Kiếm thần đã hoàn thành!");
        // bỏ hết các nguyên liệu đang trong nồi
        foreach (GameObject obj in elementalInAnvil)
        {
            Destroy(obj);
        }
        OnForgingComplete?.Invoke(true);
    }


}

