using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/* DragHandler là cho mỗi nguyên liệu Ingredient component gắn vào các prefab như cà chua, cà rốt,...
 * 
 */
public class Ingredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public IngredientSO ingredientData;
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
        iconImage = GetComponent<Image>();  
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
        AudioSource.PlayClipAtPoint(ingredientData.pronunciation, Camera.main.transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            rectTransform.position = originalPosition;
            //Debug.Log("❌ Sai chỗ");
            return;
        }

        // Tìm object có script implement IDropTarget
        var target = eventData.pointerEnter.GetComponentInParent<IDropIngredientTarget>();
        if (target != null)
        {
            //Debug.Log("✅ Thả đúng chỗ: " + target);
            target.AcceptIngredient(this);
        }
        else
        {
            rectTransform.position = originalPosition;
           // Debug.Log("❌ Sai chỗ");
        }
    }
    public string GetCraftingItem() { return ingredientData != null ? ingredientData.name : ""; }
    public void ReturnToOriginalPosition()
    {
        rectTransform.position = originalPosition;
        rectTransform.rotation = Quaternion.identity;
    }

}


