using UnityEngine;
using UnityEngine.EventSystems;

public class LandleUIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private bool canDrag = true; // 👉 mặc định cho phép kéo


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        canvas = gameObject.GetComponentInParent<Canvas>(); 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

        rectTransform.anchoredPosition = originalPosition;
    }
    // 👉 Hàm public để script khác gọi
    public void SetCanDrag(bool value)
    {
        if(!value) rectTransform.anchoredPosition = originalPosition;
        canDrag = value;
    }
}