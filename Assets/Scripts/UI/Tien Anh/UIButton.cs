using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIButton : MonoBehaviour, IPointerClickHandler
{
    private Button button;
    public string actionName; // ta sẽ gõ tên cụ thể công dụng của Button này vô. "Pause", "Mute Sound",...
    public bool canClick = true; // kiểm tra có thể bấm hay không
    public bool canKeepPress;
    private float delayTime = 0.5f; // thời gian delay giữa mỗi lần bấm để xử lý cho các button phải bấm lại liên tục 
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) return; // nếu đã bấm rồi thì bỏ qua
        canClick = false;      // đánh dấu đã bấm
        if (canKeepPress) StartCoroutine(ResetPress());
        ClickEffect(OnClick);
    }
    public void ClickEffect(System.Action onComplete = null)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale - new Vector3(0.1f, 0.1f, 0.1f);

        // Dùng Sequence để dễ thêm delay
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(targetScale, 0.1f).SetEase(Ease.InOutSine));
        seq.Append(transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBack));
        //seq.AppendInterval(0.1f); // Delay thêm giây (có thể chỉnh thành 2f nếu muốn)

        // Sau khi hiệu ứng + delay xong, gọi hành động
        if (onComplete != null)
            seq.OnComplete(() => onComplete());

        AudioManager.instance.PlaySFX("Button Pop Sound");
    }
    public void OnClick()
    {
        UIEventSystem.TriggerEvent(actionName);
    }
    private IEnumerator ResetPress()
    {
        yield return new WaitForSeconds(delayTime);
        canClick = true;
    }
    public void SetDelayTime(float delayTime)
    {
        this.delayTime = delayTime; 
    }
}
