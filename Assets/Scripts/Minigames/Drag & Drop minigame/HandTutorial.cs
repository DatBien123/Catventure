using DG.Tweening;
using UnityEngine;

public class HandTutorial : MonoBehaviour
{
    public RectTransform handTransform; // vị trí của bàn tay
    public RectTransform fromTarget;
    public RectTransform toTarget; // vị trí nồi
    public Vector2 offsetToTarget;
    public Vector2 offsetFromTarget;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayHandCookingTutorial(System.Action onComplete)
    {
        // Giờ ta cần lấy ra đĩa chứa nguyên liệu đầu tiên trong công thức của món ăn hiện tại
        fromTarget = BoilingStirringStep.Instance.GetFirstIngredientInRecipe().GetComponent<RectTransform>();
        handTransform.anchoredPosition = new Vector2(fromTarget.anchoredPosition.x + offsetFromTarget.x, fromTarget.anchoredPosition.y + offsetFromTarget.y); 
        

        // Bắt đầu tween scale phóng to thu nhỏ liên tục
        var scaleTween = handTransform
            .DOScale(3.5f, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        Vector3 targetPos = new Vector3(
        toTarget.position.x - offsetToTarget.x,
        toTarget.position.y,
        handTransform.position.z // giữ nguyên z
        );
        // Sau vài giây → di chuyển tay
        DOVirtual.DelayedCall(4f, () =>
        {
            scaleTween.Kill(); // Dừng scale
            handTransform.localScale = new Vector3(3, 3, 1);

            // Di chuyển tay từ nguyên liệu đến nồi
            handTransform.DOMove(targetPos, 3f)
                .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        // Sau khi đến → biến mất
                        handTransform.gameObject.SetActive(false);
                        onComplete?.Invoke(); // ✅ Gọi callback mở khóa

                    });
        });

    }
    //public void PlayHandForgingSwordTutorial(System.Action onComplete)
    //{
    //    // Giờ ta cần lấy ra đĩa chứa nguyên liệu đầu tiên trong công thức của món ăn hiện tại
    //    fromTarget = ForgingSwordMinigame.Instance.GetFirstElementalInRecipe().GetComponent<RectTransform>();
    //    handTransform.anchoredPosition = new Vector2(fromTarget.anchoredPosition.x + offsetFromTarget.x, fromTarget.anchoredPosition.y + offsetFromTarget.y);


    //    // Bắt đầu tween scale phóng to thu nhỏ liên tục
    //    var scaleTween = handTransform
    //        .DOScale(3.5f, 0.5f)
    //        .SetEase(Ease.InOutSine)
    //        .SetLoops(-1, LoopType.Yoyo);

    //    Vector3 targetPos = new Vector3(
    //    toTarget.position.x - offsetToTarget.x,
    //    toTarget.position.y,
    //    handTransform.position.z // giữ nguyên z
    //    );
    //    // Sau vài giây → di chuyển tay
    //    DOVirtual.DelayedCall(4f, () =>
    //    {
    //        scaleTween.Kill(); // Dừng scale
    //        handTransform.localScale = new Vector3(3, 3, 1);

    //        // Di chuyển tay từ nguyên liệu đến nồi
    //        handTransform.DOMove(targetPos, 3f)
    //            .SetEase(Ease.InOutQuad)
    //                .OnComplete(() =>
    //                {
    //                    // Sau khi đến → biến mất
    //                    handTransform.gameObject.SetActive(false);
    //                    onComplete?.Invoke(); // ✅ Gọi callback mở khóa

    //                });
    //    });

    //}
}
