using DG.Tweening;
using System.Collections.Generic;
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

        // Quét tất cả vật thể dưới con trỏ chuột
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var target = result.gameObject.GetComponentInParent<IDropIngredientTarget>();
            if (target != null)
            {
                target.AcceptIngredient(this);
                return;
            }
        }

        // Không trúng vùng nào thì quay về chỗ cũ
        rectTransform.position = originalPosition;
    }
    public string GetCraftingItem() { return ingredientData != null ? ingredientData.name : ""; }
    public void ReturnToOriginalPosition()
    {
        rectTransform.position = originalPosition;
        rectTransform.rotation = Quaternion.identity;
    }

}


