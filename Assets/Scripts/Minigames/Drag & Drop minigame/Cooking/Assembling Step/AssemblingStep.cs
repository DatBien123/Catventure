using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;

public class AssemblingStep : MonoBehaviour, ICookingStep
{
    public CookingStepSO step;
    public GameObject assemblingUI;
    public GameObject interactableObjects;
    public GameObject bowl;
    public List<GameObject> ingredients;
    public GameObject blinkEffect;

    public DragDropCookingMinigame dragDropCookingMinigame;

    public void OnEnable()
    {
        dragDropCookingMinigame.GetCountDownSystem().OnCountdownComplete += StartStep;

    }
    public void OnDisable() {
        dragDropCookingMinigame.GetCountDownSystem().OnCountdownComplete -= StartStep;

    }
    public void Start()
    {
    }
    public void CompleteStep()
    {
        Debug.Log("Đã xong bày ra món ăn");
        CookingManager.Instance.CompleteStep();
    }

    public void Setup(CookingStepSO data)
    {
        step = data;
        AudioManager.instance.PlayMusic("Chill music");
        gameObject.SetActive(true);
        dragDropCookingMinigame.GetCountDownSystem().StartCountdown321();
    }

    public void StartStep()
    {
        CanvasGroup canvasGroupBowl = bowl.GetComponent<CanvasGroup>();
        canvasGroupBowl.alpha = 0;
        RectTransform rtBowl = bowl.GetComponent<RectTransform>();
        Vector2 originalPosBowl = rtBowl.anchoredPosition;

        rtBowl.anchoredPosition = originalPosBowl + new Vector2(0, 200f);
        DG.Tweening.Sequence sq1 = DOTween.Sequence();
        sq1.Append(canvasGroupBowl.DOFade(1f, 0.2f));
        sq1.Join(rtBowl.DOAnchorPosY(originalPosBowl.y - 15f, 0.3f).SetEase(Ease.OutCubic));
        sq1.AppendCallback(() =>
        {
            AudioManager.instance.PlaySFX("Button Pop Sound");
        });
        sq1.Append(rtBowl.DOAnchorPosY(originalPosBowl.y, 0.15f).SetEase(Ease.OutBack));


        CanvasGroup canvasGroup = interactableObjects.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        RectTransform rt = interactableObjects.GetComponent<RectTransform>();
        Vector2 originalPos = rt.anchoredPosition;

        rt.anchoredPosition = originalPos + new Vector2(0, 200f);
        DG.Tweening.Sequence sq2 = DOTween.Sequence();
        sq2.Append(canvasGroup.DOFade(1f, 0.2f));
        sq2.Join(rt.DOAnchorPosY(originalPos.y - 15f, 0.3f).SetEase(Ease.OutCubic));
        sq2.AppendCallback(() =>
        {
            AudioManager.instance.PlaySFX("Button Pop Sound");
        });
        sq2.Append(rt.DOAnchorPosY(originalPos.y, 0.15f).SetEase(Ease.OutBack));

        // Show ingredient lần lượt 

        sq2.OnComplete(() => {
            ShowIngredient(0);
        });

        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(sq1);
        sequence.Append(sq2);   


    }
    public void ShowIngredient(int index)
    {
        if(index >= ingredients.Count)
        {
            return;
        }
        Ingredient ingredient = ingredients[index].transform.GetChild(0).GetComponent<Ingredient>();
        ingredient.ingredientData = step.requiredIngredients[index];
        CanvasGroup canvasGroup = ingredients[index].GetComponent<CanvasGroup>();  
        Image image = ingredients[index].transform.GetChild(0).GetComponent<Image>();   
        image.sprite = step.requiredIngredients[index].processing.assemblingData.icon;
        canvasGroup.alpha = 0;
        RectTransform rt = ingredients[index].GetComponent<RectTransform>();
        Vector2 originalPos = rt.anchoredPosition;
        rt.anchoredPosition = originalPos + new Vector2(0, 100f);

        DG.Tweening.Sequence sq = DOTween.Sequence();
        sq.Append(canvasGroup.DOFade(1, 0.1f));
        sq.Join(rt.DOAnchorPosY(originalPos.y - 15f, 0.2f).SetEase(Ease.OutCubic));
        sq.AppendCallback(() =>
        {
            AudioManager.instance.PlaySFX("Button Pop Sound");
        });
        sq.Append(rt.DOAnchorPosY(originalPos.y, 0.1f).SetEase(Ease.OutBack));
        sq.OnComplete(() => {
            ShowIngredient(index + 1);
        });
    }
    public void HideIngredients(int index)
    {
        if (index >= ingredients.Count)
        {
            return;
        }
        CanvasGroup canvasGroup = ingredients[index].GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        RectTransform rt = ingredients[index].GetComponent<RectTransform>();
        //Vector2 originalPos = rt.anchoredPosition;
        //rt.anchoredPosition = originalPos + new Vector2(0, 100f);

        DG.Tweening.Sequence sq = DOTween.Sequence();
        sq.Append(canvasGroup.DOFade(0, 0.1f));
        //sq.Join(rt.DOAnchorPosY(originalPos.y - 15f, 0.2f).SetEase(Ease.OutCubic));
        //sq.AppendCallback(() =>
        //{
        //    AudioManager.instance.PlaySFX("Button Pop Sound");
        //});
        //sq.Append(rt.DOAnchorPosY(originalPos.y, 0.1f).SetEase(Ease.OutBack));
            HideIngredients(index + 1);
    }

    public void ShowDishCompleted()
    {
        HideIngredients(0);
        HideInteractableObjects();


        RectTransform rt = bowl.GetComponent<RectTransform>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rt.DOScale(rt.localScale + new Vector3(0.2f, 0.2f, 0.2f), 1f));
        sequence.OnComplete(() => {
            GameObject effect = Instantiate(blinkEffect, assemblingUI.transform);
            effect.GetComponent<EffectSprite>().ShowEffect(2f);
            StartCoroutine(PlaySoundCompletedStep());
        });

    }

    private void HideInteractableObjects()
    {
        CanvasGroup canvasGroup = interactableObjects.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        DG.Tweening.Sequence sq = DOTween.Sequence();
        sq.Append(canvasGroup.DOFade(0, 0.1f));
    }
    IEnumerator PlaySoundCompletedStep()
    {
        AudioManager.instance.PlaySFX("Shine Sound");
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlaySFX("Level Win");
        yield return new WaitForSeconds(3f);
        CompleteStep();
    }
}
