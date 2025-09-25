using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KnifeUIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private bool canDrag = true; // 👉 mặc định cho phép kéo

    [Header("Sprites")]
    public Sprite knifeIdle;
    public Sprite knifeActive;
    private Image image;


    [Header("Controller")]
    public ArrowSwipeController swipeController; // Gán trong Inspector

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        canvas = gameObject.GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        image.sprite = knifeIdle;
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

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // chặn

        swipeController.OnKnifeDragEnd();
        rectTransform.anchoredPosition = originalPosition;
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
