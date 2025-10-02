using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIHome : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public AudioManager audioManager;
    public UIMapDescription UIMapDescription;
    public GameObject MapLockedNotify;


    public RectTransform[] Maps;

    public float DelayTime = 1.2f;
    private void OnEnable()
    {
        animator.CrossFadeInFixedTime("Home Open", 0.0f);
        StartCoroutine(DelaySound());

        AnimateMaps(); // 👈 gọi hiệu ứng cho các Map
    }

    IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(DelayTime);

        if(audioManager != null)
        {
            audioManager.PlaySFX("Background Music");
        }
    }
    void AnimateMaps()
    {
        foreach (var map in Maps)
        {
            if (map == null) continue;

            // Ghi nhớ vị trí & scale ban đầu để dễ reset
            Vector3 startPos = map.anchoredPosition;
            Vector3 startScale = map.localScale;

            // ✨ Di chuyển lên xuống nhẹ (ví dụ 15px)
            map.DOAnchorPosY(startPos.y + 15f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetDelay(Random.Range(0f, 0.5f))
                .SetLoops(-1, LoopType.Yoyo); // -1 = vô hạn, Yoyo = đi lên rồi quay lại

            // ✨ Scale nhẹ lên xuống (ví dụ phóng to 1.05 lần)
            map.DOScale(startScale * 1.05f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
