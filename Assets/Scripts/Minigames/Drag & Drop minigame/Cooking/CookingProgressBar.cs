using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CookingProgressBar : MonoBehaviour
{
    public Slider slider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCookingProgress(float amount, float duration)
    {
        float targetValue = Mathf.Min(slider.value + amount, slider.maxValue);
        slider.DOValue(targetValue, duration).SetEase(Ease.OutCubic);

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale + new Vector3(0.1f, 0.1f, 0.1f);

        // Dùng Sequence để dễ thêm delay
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(targetScale, 0.1f).SetEase(Ease.InOutSine));
        seq.Append(transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack));
        AudioManager.instance.PlaySFX("Button Pop Sound");
    }
}
