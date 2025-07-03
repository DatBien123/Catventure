using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    public Image imageComponent;
    public TextMeshProUGUI contentTextComponent;

    public bool isSelected;

    [Header("Button Selected")]
    public Sprite backgroundSelected;
    public Color contentColorSelected;

    [Header("Button Deselected")]
    public Sprite backgroundDeselected;
    public Color contentColorDeselected;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        contentTextComponent = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        ResetDefault();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;
        if(backgroundSelected != null && contentColorSelected != null)
        {
            imageComponent.sprite = backgroundSelected;
            contentTextComponent.color = contentColorSelected;
        }

    }

    public void OnPointerUp(PointerEventData eventData){}
    public void ResetDefault()
    {
        isSelected=false;
        if (backgroundDeselected != null && contentColorDeselected != null)
        {
            imageComponent.sprite = backgroundDeselected;
            contentTextComponent.color = contentColorDeselected;
        }
    }
}
