using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public enum FilterType
{
    All,
    Equipment,
    Consumable,
    Material,
    Quest,
    Misc
}

public class UIInventory : MonoBehaviour
{
    public Inventory inventoryManager;

    [Header("UI")]
    public GameObject slotPrefab;
    public Transform slotParent;

    [Header("Tab Buttons")]
    public Button buttonAll;
    public Button buttonEquipment;
    public Button buttonConsumable;
    public Button buttonMaterial;
    public Button buttonQuest;
    public Button buttonMisc;

    public List<UIInventorySlot> uiSlots = new List<UIInventorySlot>();

    public UIInventorySlot currentInventorySlotSelected;
    public UIItemInfo itemInfo;

    private FilterType currentFilter = FilterType.All;

    private void Start()
    {
        buttonAll.onClick.AddListener(() => ChangeFilter(FilterType.All));
        buttonEquipment.onClick.AddListener(() => ChangeFilter(FilterType.Equipment));
        buttonConsumable.onClick.AddListener(() => ChangeFilter(FilterType.Consumable));
        buttonMaterial.onClick.AddListener(() => ChangeFilter(FilterType.Material));
        buttonQuest.onClick.AddListener(() => ChangeFilter(FilterType.Quest));
        buttonMisc.onClick.AddListener(() => ChangeFilter(FilterType.Misc));

        RefreshUI();


        AddItem();
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

        // Lọc dữ liệu inventory theo filter
        var filteredSlots = inventoryManager.slots.Where(slot =>
            currentFilter == FilterType.All ||
            (currentFilter == FilterType.Equipment && slot.item.data.itemType == ItemType.Equipment) ||
            (currentFilter == FilterType.Consumable && slot.item.data.itemType == ItemType.Consumable) ||
            (currentFilter == FilterType.Material && slot.item.data.itemType == ItemType.Material) ||
            (currentFilter == FilterType.Quest && slot.item.data.itemType == ItemType.Quest) ||
            (currentFilter == FilterType.Misc && slot.item.data.itemType == ItemType.Misc)
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

    //Test
    public SO_Item item1;
    public SO_Item item2;
    public SO_Item item3;
    public SO_Item item4;
    public SO_Item item5;
    public SO_Item item6;
    public int quantity;
    public void AddItem()
    {
        inventoryManager.AddItem(item1, quantity);
        inventoryManager.AddItem(item2, quantity);
        inventoryManager.AddItem(item3, quantity);
        inventoryManager.AddItem(item4, quantity);
        inventoryManager.AddItem(item5, quantity);
        inventoryManager.AddItem(item6, quantity);
        RefreshUI();
    }


}
