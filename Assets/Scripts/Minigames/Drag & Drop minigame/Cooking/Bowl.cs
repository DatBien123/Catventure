using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

public class Bowl : MonoBehaviour, IDropIngredientTarget
{
    public List<GameObject> ingredientsInBowl;
    public AssemblingStep assemblingStep;

    public void Start()
    {
        Transform current = gameObject.transform;

        // Lấy cha trực tiếp
        Transform parent = current.parent;

        // Lấy cha của cha
        Transform grandParent = current.parent.parent;

        // Nếu muốn lấy GameObject thay vì Transform
        GameObject grandParentObj = current.parent.parent.gameObject;
        assemblingStep = grandParentObj.GetComponent<AssemblingStep>(); 
    }
    public void AcceptIngredient(Ingredient ingredient)
    {
        ingredient.gameObject.SetActive(false); 
        Debug.Log("Thả vào bát");
        Debug.Log(ingredient.ingredientData.ingredientName);
        GameObject ingredientInPot = ingredientsInBowl.Find(obj => obj.name == ingredient.ingredientData.ingredientName);
        if (ingredientInPot != null)
        {
            Debug.Log("✅ Đã tìm thấy trong bát: " + ingredientInPot.name);
            ingredientInPot.SetActive(true);
            CanvasGroup canvasGroup = ingredientInPot.GetComponent<CanvasGroup>();
            RectTransform rt = ingredientInPot.GetComponent<RectTransform>();
            Vector2 originalPos = rt.anchoredPosition;
            rt.anchoredPosition = originalPos + new Vector2(0, 100f);

            DG.Tweening.Sequence sq = DOTween.Sequence();
            sq.Append(canvasGroup.DOFade(1f, 0.2f));
            sq.Join(rt.DOAnchorPosY(originalPos.y - 15f, 0.3f).SetEase(Ease.OutCubic));
            sq.AppendCallback(() =>
            {
                AudioManager.instance.PlaySFX("Button Pop Sound");
            });
            sq.Append(rt.DOAnchorPosY(originalPos.y, 0.15f).SetEase(Ease.OutBack));
            if (CheckAllActive()) assemblingStep.ShowDishCompleted();

        }
        else
        {
            Debug.Log("❌ Không có " + ingredient.ingredientData.ingredientName + " trong bát");
        }

    }

    public bool CheckAllActive()
    {
        if (ingredientsInBowl.All(obj => obj.activeSelf))
        {
            Debug.Log("✅ Tất cả nguyên liệu đã active!");
            return true;
        }
        else
        {
            Debug.Log("❌ Vẫn còn nguyên liệu chưa active");
            return false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
