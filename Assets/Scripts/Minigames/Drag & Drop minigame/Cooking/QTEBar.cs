using DG.Tweening;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
public class QTEBar : MonoBehaviour
{
    public Transform pointA; // điểm A là điểm bắt đầu
    public Transform pointB; // điểm B là điểm cuối
    public RectTransform greenZone;
    public RectTransform redZone;
    public RectTransform yellowZone;
    public RectTransform pointerTransform; // cái thanh sẽ di chuyển qua lại để ta chơi và ấn đúng lúc
    public Transform targetPosition; // điểm đến tiếp theo của Pointer
    public float moveSpeed; // tốc độ di chuyển của Pointer ta có thể tăng lên sau mỗi lần ấn đúng

    // Các thành UI của QTE
    // Thanh progress bar thể hiện qui trình nấu 
    public CookingProgressBar cookingProgress;
    public Button cookButton;
    public UIButton QTEPressButton; // nút bấm để ta chơi QTE Bar
    // các sự kiện báo cho lớp khác xử lý UI
    public Action onCookingCompleted;
    public static Action onPerfectZone;
    public static Action onGoodZone;
    private bool canPress = true;

    private void OnEnable()
    {
        UIEventSystem.Register("QTEPress", CheckSuccess);
    }
    private void OnDisable()
    {
        UIEventSystem.Unregister("QTEPress", CheckSuccess);

    }
    private void Start()
    {
        cookingProgress.slider.onValueChanged.AddListener(OnCookingProgressChanged);
        QTEPressButton.SetDelayTime(1.5f);
    }

    private void OnCookingProgressChanged(float value)
    {
        if (value >= cookingProgress.slider.maxValue) // thường là 100
        {
            //Debug.Log("🎉 Nấu xong rồi!");
            onCookingCompleted?.Invoke();
            // Gọi sự kiện thắng game, mở UI, vv...
        }
    }

    private void Update()
    {
        // di chuyển Pointer đến TargetPosition ở đây là điểm A hoặc điểm B
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        // Nếu đã ở điểm A thì targetPosition điểm phải đến tiếp theo là điểm B
        if(Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f) // hàm kiểm tra khoảng cách giữa pointer và điểm A
        {
            targetPosition = pointB;
          
        }
        // Nếu ở điểm B rồi thì target Position điểm phải đến tiếp theo là A
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f){
            targetPosition = pointA;    
        }


    }

    public void CheckSuccess()
    {
        if (!canPress) return; // đang pause thì bỏ qua
        Image pointerImage = pointerTransform.GetComponent<Image>();
        Color originalColor = pointerImage.color;
        // khi người chơi bấm nút ta phải check Pointer hiện tại ở Green Zone, Yellow Zone hay là điểm đen
        if (RectTransformUtility.RectangleContainsScreenPoint(greenZone, pointerTransform.position, null))
        {
            cookingProgress.AddCookingProgress(25, 0.5f);
            moveSpeed += 100;
            AudioManager.instance.PlaySFX("Perfect Zone");
            onPerfectZone?.Invoke();

            // DỪNG pointer trong 0.5s
            StartCoroutine(PausePointer(0.5f));

            Vector3 originalScale = pointerTransform.localScale;
            Vector3 targetScale = originalScale * 1.3f;

            DG.Tweening.Sequence seq = DOTween.Sequence();
            seq.Append(pointerTransform.DOScale(targetScale, 0.2f).SetEase(Ease.OutBack));
            seq.Join(pointerImage.DOColor(Color.green, 0.2f)); // đổi sang xanh
            seq.AppendInterval(0.3f); // giữ xanh 0.5s
            seq.Append(pointerTransform.DOScale(originalScale, 0.2f).SetEase(Ease.InOutSine));
            seq.Join(pointerImage.DOColor(originalColor, 0.2f)); // trả lại màu gốc
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(yellowZone, pointerTransform.position, null))
        {
            cookingProgress.AddCookingProgress(10, 0.5f);
            moveSpeed += 100;
            AudioManager.instance.PlaySFX("Good Zone");
            onGoodZone?.Invoke();

            StartCoroutine(PausePointer(0.3f));

            Vector3 originalScale = pointerTransform.localScale;
            Vector3 targetScale = originalScale * 1.1f;

            DG.Tweening.Sequence seq = DOTween.Sequence();
            seq.Append(pointerTransform.DOScale(targetScale, 0.1f).SetEase(Ease.InOutSine));
            seq.Join(pointerImage.DOColor(Color.yellow, 0.1f)); // đổi sang vàng
            seq.AppendInterval(0.15f); // giữ vàng 0.3s
            seq.Append(pointerTransform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBack));
            seq.Join(pointerImage.DOColor(originalColor, 0.1f)); // trả lại màu gốc
        }

        else
        {
            Debug.Log("Red Zone");
        }

    }


    public void SetupQTEBar()
    {
        gameObject.SetActive(true);
        cookButton.gameObject.GetComponent<Image>().DOFade(255f, 2f);
    }
    private IEnumerator PausePointer(float duration)
    {
        float oldSpeed = moveSpeed;
        moveSpeed = 0;
        canPress = false; // khóa input

        yield return new WaitForSeconds(duration);

        moveSpeed = oldSpeed;
        canPress = true; // mở input lại
    }


}
