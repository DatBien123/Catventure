using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour,IObjectPool<UIInventorySlot>, IPointerClickHandler
{

    [Header("UI Components")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public Outline Outline;

    public GameObject EquipedImage;

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

    public int poolID { get; set; }
    public ObjectPooler<UIInventorySlot> pool { get; set; }

    private void Awake()
    {
        uiInventory = GameObject.FindAnyObjectByType<UIInventory>();
        backGroundImage = GetComponent<Image>();

        Outline.enabled = false;
    }
    #region [Interaction]
    public void OnPointerClick(PointerEventData eventData)
    {
        //Loop through all these slots of inventory
        //Deselect All
        foreach(var uiSlot in uiInventory.uiSlots)
        {
            uiSlot.isSelected = false;
            uiSlot.Outline.enabled = false;
        }
        uiInventory.currentInventorySlotSelected = this;
        uiInventory.ShowActionButton(this.slotData.ItemInstance);
        isSelected = true;
        Outline.enabled = true;
    }
    #endregion
    public void SetupSlot(InventorySlot slot)
    {
        slotData = slot;

        //Set up Grid Inventory
        iconImage.sprite = slot.ItemInstance.ItemStaticData.commonData.icon;
        iconImage.enabled = true;

        if (slot.ItemInstance.ItemStaticData.commonData.isStackable && slot.ItemInstance.Quantity > 1)
            quantityText.text = slot.ItemInstance.Quantity.ToString();
        else
            quantityText.text = "";

        //Set up Description
        Name = slot.ItemInstance.ItemStaticData.commonData.itemName;
        Type = "(" + slot.ItemInstance.ItemStaticData.commonData.itemType.ToString() + ")";
        Description = slot.ItemInstance.ItemStaticData.commonData.description;

        if(slot.ItemInstance.IsEquiped == true)
        {
            EquipedImage.gameObject.SetActive(true);
        }
        else
        {
            EquipedImage.gameObject.SetActive(false);
        }
    }
    public void ClearSlot()
    {
        slotData = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
        quantityText.text = "";
    }
}

