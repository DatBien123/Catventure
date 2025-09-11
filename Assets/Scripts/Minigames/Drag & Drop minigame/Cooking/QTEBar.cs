using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
public class QTEBar : MonoBehaviour
{
    public Transform pointA; // điểm A là điểm bắt đầu
    public Transform pointB; // điểm B là điểm cuối
    public RectTransform greenZone;
    public RectTransform yellowZone1;
    public RectTransform yellowZone2;
    public RectTransform pointerTransform; // cái thanh sẽ di chuyển qua lại để ta chơi và ấn đúng lúc
    public Transform targetPosition; // điểm đến tiếp theo của Pointer
    public float moveSpeed; // tốc độ di chuyển của Pointer ta có thể tăng lên sau mỗi lần ấn đúng

    // Các thành UI của QTE
    // Thanh progress bar thể hiện qui trình nấu 
    public Slider cookingProgress;
    bool canPress = true;
    public Button cookButton;
    public Action onCookingCompleted;
    public static Action onPerfectZone;
    public static Action onGoodZone;

    private void Start()
    {
        cookingProgress.onValueChanged.AddListener(OnCookingProgressChanged);
    }

    private void OnCookingProgressChanged(float value)
    {
        if (value >= cookingProgress.maxValue) // thường là 100
        {
            Debug.Log("🎉 Nấu xong rồi!");
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
        if (Input.GetKeyDown(KeyCode.Space) && canPress)
        {
            canPress = false;
            CheckSuccess();
            }

    }

    public void CheckSuccess()
    {
        // khi người chơi bấm nút ta phải check Pointer hiện tại ở Green Zone, Yellow Zone hay là điểm đen
        if(RectTransformUtility.RectangleContainsScreenPoint(greenZone, pointerTransform.position, null))
        {
            AddCookingProgress(25, 0.5f);
            moveSpeed += 100;
            AudioManager.instance.PlaySFX("Perfect Zone");
            // báo sự kiện cho nồi ở đây để nó chạy hiệu ứng
            onPerfectZone?.Invoke();
            
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(yellowZone1, pointerTransform.position, null) || RectTransformUtility.RectangleContainsScreenPoint(yellowZone2, pointerTransform.position, null))
        {
            AddCookingProgress(10, 0.5f);
            moveSpeed += 100;
            AudioManager.instance.PlaySFX("Good Zone");
            // báo sự kiện cho nồi ở đây để nó chạy hiệu ứng
            onGoodZone?.Invoke();

        }

        else
        {
        }
        Invoke(nameof(ResetPress), 1f);

    }
    public void ResetPress()
    {
        canPress = true;
    }

    public void SetupQTEBar()
    {
        gameObject.SetActive(true);
        cookButton.gameObject.GetComponent<Image>().DOFade(255f, 2f);
    }

    public void AddCookingProgress(float amount, float duration)
    {
        float targetValue = Mathf.Min(cookingProgress.value + amount, cookingProgress.maxValue);
        cookingProgress.DOValue(targetValue, duration).SetEase(Ease.OutCubic);

        Vector3 originalScale = cookingProgress.transform.localScale;
        Vector3 targetScale = originalScale + new Vector3(0.1f, 0.1f, 0.1f);

        // Dùng Sequence để dễ thêm delay
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(cookingProgress.transform.DOScale(targetScale, 0.1f).SetEase(Ease.InOutSine));
        seq.Append(cookingProgress.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack));
        AudioManager.instance.PlaySFX("Button Pop Sound");
    }
    
}
