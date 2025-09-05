using DG.Tweening;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemBarUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform iconContainer;
    private Dictionary<CraftingItemSO, GameObject> craftingItemIcons = new();


    public void Init(CraftingRecipeSO recipe)
    {
        // lặp qua các nguyên liệu cần để sinh ra object con trong ingredient bar
        foreach(CraftingItemSO ingredient in recipe.requiredItems)
        {
            GameObject icon = Instantiate(iconPrefab, iconContainer);
            icon.GetComponentInChildren<Image>().sprite = ingredient.icon;
            icon.GetComponent<CanvasGroup>().alpha = 0.3f;
            craftingItemIcons.Add(ingredient, icon);
        }
    }
    public void MarkItemAsCollected(CraftingItemSO ingredient)
    {
        if (craftingItemIcons.TryGetValue(ingredient, out GameObject obj))
        {
            obj.GetComponent<CanvasGroup>().alpha = 1f; // hiện rõ
            obj.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo); // hiệu ứng bật nhẹ
        }
    }

}
