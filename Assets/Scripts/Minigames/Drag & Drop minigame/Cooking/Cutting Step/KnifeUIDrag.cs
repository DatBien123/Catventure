using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KnifeUIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalPosition;

    private RectTransform rectTransform;

    [Header("Sprites")]
    public Sprite knifeIdle;
    public Sprite knifeActive;
    private Image image;
    private bool canDrag = true; // 👉 mặc định cho phép kéo


    [Header("Controller")]
    public ArrowSwipeController swipeController; // Gán trong Inspector

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.sprite = knifeIdle;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

        image.sprite = knifeActive;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn
        image.sprite = knifeIdle;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

        swipeController.OnKnifeDragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

        transform.position = eventData.position; // Dao đi theo tay/chuột
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

        swipeController.OnKnifeDragEnd();
        transform.position = originalPosition;
        image.sprite = knifeIdle;
    }
    public void SetCanDrag(bool value)
    {
        if (!value)
        { rectTransform.anchoredPosition = originalPosition;
            image.sprite = knifeIdle;

        }
        canDrag = value;
    }
}
