using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/* DragHandler là cho mỗi nguyên liệu Ingredient component gắn vào các prefab như cà chua, cà rốt,...
 * 
 */
public class CraftingItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public CraftingItemSO crafttingItemData;
    public Image iconImage;
    [SerializeField]private Canvas canvas;
    [SerializeField] public RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Vector3 originalPosition;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.FindObjectOfType<Canvas>();
    }
    void Start()
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        canvasGroup.blocksRaycasts = false; // để DropZone nhận được sự kiện
        AudioSource.PlayClipAtPoint(crafttingItemData.pronunciation, Camera.main.transform.position);
        Debug.Log("Đã ấn trúng nguyên liệu hãy phát tiếng");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        // Nếu không thả vào đúng chỗ → quay lại vị trí cũ
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponentInParent<Pot>() == null)
        {
            rectTransform.position = originalPosition;
            Debug.Log("❌ Sai chỗ");
        }
        else
        {
            Debug.Log("Thả nguyên liệu đúng chỗ");
        }
    }
    public string GetCraftingItem() { return crafttingItemData != null ? crafttingItemData.name : ""; }
    public void ReturnToOriginalPosition()
    {
        rectTransform.position = originalPosition;
        rectTransform.rotation = Quaternion.identity;
    }

}


