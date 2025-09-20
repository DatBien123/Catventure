using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
public class CuttingStepUI : MonoBehaviour
{
    public Image ingredientImage;
    public RectTransform kitchenKnife; // con dao bếp

    public GameObject ingredientName;
    public Text ingredientNameText;

    public GameObject finishedText;
    public CraftingItemBarUI ingredientBarUI;


    public void SetupUI(CookingStepSO cookingStep)
    {
        //this.ingredientImage.sprite = ingredientImage;
        ingredientBarUI.InitBar(cookingStep);
    }
    //public void UpdateUI() { } hàm này ta có thể dùng để Update UI khi có thay đổi gì đó
    
    public void ShowCutted(Sprite cuttedSprite)
    {
        // Show ra hình ảnh nguyên liệu đã thái xong
    }
    public void ShowIngredientName(string name)
    {
        ingredientName.SetActive(true);
        RectTransform rt = ingredientName.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        rt.DOScale(1.2f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                rt.DOScale(1f, 0.1f)
      .SetEase(Ease.InOutSine);
            }

            );
        
        ingredientNameText.text = name;
    }
    public void HideIngredientName()
    {
        RectTransform rt = ingredientName.GetComponent<RectTransform>();
        rt.DOScale(0f, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                ingredientName.SetActive(false);
            });
    }



}
