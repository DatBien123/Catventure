using DG.Tweening;
using UnityEngine;

public class FinishedTextUI : MonoBehaviour
{
    public void ShowFinishedText()
    {
        GameObject parent = transform.parent.gameObject;
        parent.SetActive(true);
        AudioManager.instance.PlaySFX("Happy Sound");
        Debug.Log("Chạy Happy Sound");
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        rt.DOScale(1.2f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                rt.DOScale(1f, 0.1f)
                .SetEase(Ease.InOutSine);
            }
            );
    }
    public void HideFinishedText()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.DOScale(0f, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                GameObject parent = transform.parent.gameObject;
                parent.SetActive(false);
            });
    }
}
