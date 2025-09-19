using DG.Tweening;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemBarUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform iconContainer;
    private Dictionary<IngredientSO, GameObject> craftingItemIcons = new();


    //public void Init(IngredientRecipeSO recipe)
    //{
    //    // lặp qua các nguyên liệu cần để sinh ra object con trong ingredient bar
    //    foreach(IngredientSO ingredient in recipe.requiredItems)
    //    {
    //        GameObject icon = Instantiate(iconPrefab, iconContainer);
    //        icon.GetComponentInChildren<Image>().sprite = ingredient.icon;
    //        icon.GetComponent<CanvasGroup>().alpha = 0.3f;
    //        craftingItemIcons.Add(ingredient, icon);
    //    }
    //}
    public void InitBar(CookingStepSO step)
    {
        // lặp qua các nguyên liệu cần để sinh ra object con trong ingredient bar
        foreach (IngredientSO ingredient in step.requiredIngredients)
        {
            GameObject icon = Instantiate(iconPrefab, iconContainer);
            icon.GetComponentInChildren<Image>().sprite = ingredient.icon;
            icon.GetComponent<CanvasGroup>().alpha = 0.3f;
            craftingItemIcons.Add(ingredient, icon);
        }
    }
    public void MarkItemAsCollected(IngredientSO ingredient)
    {
        if (craftingItemIcons.TryGetValue(ingredient, out GameObject obj))
        {
            obj.GetComponent<CanvasGroup>().alpha = 1f; // hiện rõ
            obj.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo); // hiệu ứng bật nhẹ
        }
    }

}
