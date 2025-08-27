using DG.Tweening;
using UnityEngine;

public class ImageAnimations : MonoBehaviour
{
    [Header("Scale Settings")]
    public float scaleUpValue = 1.2f;    // tỉ lệ scale khi phóng to
    public float duration = 0.5f;        // thời gian cho mỗi lần scale

    private void Start()
    {
        // Đảm bảo scale ban đầu là (1,1,1)
        transform.localScale = Vector3.one;

        // Tween scale lên -> xuống liên tục
        transform.DOScale(scaleUpValue, duration)
                 .SetEase(Ease.InOutSine)            // mượt mà
                 .SetLoops(-1, LoopType.Yoyo);       // lặp vô tận, kiểu qua lại
    }
}
