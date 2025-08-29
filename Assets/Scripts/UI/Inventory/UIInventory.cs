using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public enum FilterType
{
    Shirt,
    Trouser,
    Shoes,
    Hat,
    Consumable,

}

public class UIInventory : MonoBehaviour
{
    public Inventory inventoryManager;

    [Header("UI")]
    public GameObject slotPrefab;
    public Transform slotParent;

    [Header("Tab Buttons")]
    public Button buttonShirt;
    public Button buttonTrouser;
    public Button buttonShoes;
    public Button buttonHat;
    public Button buttonConsumable;


    public List<UIInventorySlot> uiSlots = new List<UIInventorySlot>();

    public UIInventorySlot currentInventorySlotSelected;
    public UIInventoryActions uiInventoryActions;
    public UIItemInfo itemInfo;

    private FilterType currentFilter = FilterType.Shirt;

    private void Start()
    {
        buttonShirt.onClick.AddListener(() => ChangeFilter(FilterType.Shirt));
        buttonTrouser.onClick.AddListener(() => ChangeFilter(FilterType.Trouser));
        buttonShoes.onClick.AddListener(() => ChangeFilter(FilterType.Shoes));
        buttonHat.onClick.AddListener(() => ChangeFilter(FilterType.Hat));
        buttonConsumable.onClick.AddListener(() => ChangeFilter(FilterType.Consumable));



        RefreshUI();

        //Ẩn item info khi bắt đầu
        itemInfo.gameObject.SetActive(false);

    }

    public void ChangeFilter(FilterType filter)
    {
        currentFilter = filter;
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Xoá hết UI Slot cũ
        foreach (var slot in uiSlots)
        {
            Destroy(slot.gameObject);
        }
        uiSlots.Clear();

        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        // Lọc dữ liệu inventory theo filter
        var filteredSlots = inventoryManager.slots.Where(slot =>
            (currentFilter == FilterType.Shirt && slot.item.commonData.itemType == ItemType.Shirt) ||
            (currentFilter == FilterType.Trouser && slot.item.commonData.itemType == ItemType.Trouser) ||
            (currentFilter == FilterType.Shoes && slot.item.commonData.itemType == ItemType.Shoes) ||
            (currentFilter == FilterType.Hat && slot.item.commonData.itemType == ItemType.Hat) ||
            (currentFilter == FilterType.Consumable && slot.item.commonData.itemType == ItemType.Consumable) 
        );

        // Tạo UI Slot mới dựa trên dữ liệu đã lọc
        foreach (var invSlot in filteredSlots)
        {
            GameObject obj = Instantiate(slotPrefab, slotParent);
            UIInventorySlot uiSlot = obj.GetComponent<UIInventorySlot>();
            uiSlot.SetupSlot(invSlot);
            uiSlots.Add(uiSlot);
        }
    }

    public void RefreshItemDescriptionInfo()
    {
        if(currentInventorySlotSelected == null)
        {
            itemInfo.ClearInfo();
        }

        itemInfo.ShowInfo(currentInventorySlotSelected.slotData.item);

    }

    ////Test
    //public SO_Item item1;
    //public SO_Item item2;

    //public int quantity;
    //public void AddItem()
    //{
    //    inventoryManager.AddItem(item1, quantity);
    //    inventoryManager.AddItem(item2, quantity);

    //    RefreshUI();
    //}


}
