using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class AnswerButtonEffect : MonoBehaviour
{
    [SerializeField] private Image bigCorrectCheckMark;
    [SerializeField] private Image smallCorrectCheckMark;
    [SerializeField] private Image bigWrongCheckMark;
    [SerializeField] private Image smallWrongCheckMark;
    [SerializeField] private CanvasGroup canvasGroup;
    public GameObject confettiEffect;



    public void ResetEffects()
    {
        bigCorrectCheckMark.gameObject.SetActive(false);
        smallCorrectCheckMark.gameObject.SetActive(false);
        bigWrongCheckMark.gameObject.SetActive(false);
        smallWrongCheckMark.gameObject.SetActive(false);
        canvasGroup.alpha = 1f;
    }

    public void ShowCorrectEffect()
    {
        bigCorrectCheckMark.gameObject.SetActive(true);
        bigCorrectCheckMark.color = new Color(1, 1, 1, 1);
        bigCorrectCheckMark.rectTransform.anchoredPosition = Vector2.zero;

        Sequence s = DOTween.Sequence();
        s.Append(bigCorrectCheckMark.rectTransform.DOAnchorPosY(200f, 1f).SetEase(Ease.OutQuad))
                     .InsertCallback(0.5f, () =>
                     {
                         GameObject effect = Instantiate(confettiEffect, gameObject.transform);
                     })
         .Join(bigCorrectCheckMark.DOFade(0f, 3f));

        s.OnComplete(() => {
        });
        smallCorrectCheckMark.gameObject.SetActive(true);
        smallCorrectCheckMark.DOFade(0.5f, 0.3f);
    }
    public void ClickEffect()
    {
        // hiệu ứng lõm xuống khi bấm
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale + new Vector3(0.1f, 0.1f, 0.1f);
        transform.DOScale(targetScale, 0.1f)
        .SetEase(Ease.InOutSine)
        .OnComplete(() => {
            transform.DOScale(originalScale, 0.2f)
                     .SetEase(Ease.OutBack);
        });

    }
    public void ShowWrongEffect()
    {
        bigWrongCheckMark.gameObject.SetActive(true);
        bigWrongCheckMark.color = new Color(1, 1, 1, 1);
        bigWrongCheckMark.rectTransform.anchoredPosition = Vector2.zero;

        Sequence s = DOTween.Sequence();
        s.Append(bigWrongCheckMark.rectTransform.DOAnchorPosY(200f, 1f).SetEase(Ease.OutQuad))
            .Join(bigWrongCheckMark.DOFade(0f, 3f));

        smallWrongCheckMark.gameObject.SetActive(true);
        smallWrongCheckMark.DOFade(0.5f, 0.3f);

    }
    public void FadeOut()
    {
        //GetComponent<Button>().interactable = false;
        canvasGroup.DOFade(0.5f, 0.25f)
            .SetDelay(1f);
    }
    public void ShowOnlySmallCheck()
    {
        smallCorrectCheckMark.gameObject.SetActive(true);
        smallCorrectCheckMark.DOFade(0.5f, 0.3f);
    }
}
