using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerDownHandler
{

    [Header("UI Components")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    [Header("UI Description")]
    public string Name;
    public string Type;
    public string Description;

    [Header("On Interact")]
    public Image backGroundImage;
    public bool isSelected;

    [HideInInspector]
    public InventorySlot slotData;

    [Header("Reference")]
    public UIInventory uiInventory;

    private void Awake()
    {
        uiInventory = GameObject.FindAnyObjectByType<UIInventory>();
        backGroundImage = GetComponent<Image>();
    }
    #region [Interaction]
    public void OnPointerDown(PointerEventData eventData)
    {
        //Loop through all these slots of inventory
        //Deselect All
        foreach(var uiSlot in uiInventory.uiSlots)
        {
            uiSlot.isSelected = false;
            uiSlot.OnSelect(false);
        }

        //uiInventory.itemInfo.gameObject.SetActive(true);
        uiInventory.currentInventorySlotSelected = this;
        isSelected = true;
        OnSelect(true);
        uiInventory.RefreshItemDescriptionInfo();

    }
    #endregion
    public void SetupSlot(InventorySlot slot)
    {
        slotData = slot;

        //Set up Grid Inventory
        iconImage.sprite = slot.item.commonData.icon;
        iconImage.enabled = true;

        if (slot.item.commonData.isStackable && slot.currentQuantity > 1)
            quantityText.text = slot.currentQuantity.ToString();
        else
            quantityText.text = "";

        //Set up Description
        Name = slot.item.commonData.itemName;
        Type = "(" + slot.item.commonData.itemType.ToString() + ")";
        Description = slot.item.commonData.description;

    }
    public void ClearSlot()
    {
        slotData = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
        quantityText.text = "";
    }

    public void OnSelect(bool isSelect)
    {
        if (isSelect)
        {
            backGroundImage.sprite = slotData.item.displayData.buttonData.selectFieldData.backgroundSprite;
            backGroundImage.color = slotData.item.displayData.buttonData.selectFieldData.backgroundColor;
        }
        else
        {
            backGroundImage.sprite = slotData.item.displayData.buttonData.deselectdFieldData.backgroundSprite;
            backGroundImage.color = slotData.item.displayData.buttonData.deselectdFieldData.backgroundColor;
        }
    }
}

