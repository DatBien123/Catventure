using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public enum FilterType
{
    Shirt,
    Glasses,
    HandStuff,
    Hat,
    Consumable,
    Wing

}

public class UIInventory : MonoBehaviour
{
    public Inventory inventoryManager;

    [Header("UI")]
    public UIInventorySlot slotPrefab;
    public Transform slotParent;

    [Header("Filter Buttons")]
    public Button buttonShirt;
    public Button buttonHandStuff;
    public Button buttonGlasses;
    public Button buttonHat;
    public Button buttonWing;
    public Button buttonConsumable;

    [Header("Interact Button")]
    public Button buttonWear;
    public Button buttonTakeoff;

    [Header("Data")]
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Coin;

    public List<UIInventorySlot> uiSlots = new List<UIInventorySlot>();

    public UIInventorySlot currentInventorySlotSelected;
    public UIInventoryActions uiInventoryActions;

    private FilterType currentFilter = FilterType.Shirt;

    public UnityEvent OnWear;
    public UnityEvent OnTakeOff;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 100;
    protected ObjectPooler<UIInventorySlot> pooler { get; private set; }
    //protected GameObject poolParent;

    #endregion

    private void OnEnable()
    {
        RefreshUI();
    }
    private void Awake()
    {
        pooler = new ObjectPooler<UIInventorySlot>();
        pooler.Initialize(this, poolCount, slotPrefab, slotParent);
    }

    private void Start()
    {
        //Filter Register
        buttonShirt.onClick.AddListener(() => ChangeFilter(FilterType.Shirt));
        buttonGlasses.onClick.AddListener(() => ChangeFilter(FilterType.Glasses));
        buttonHandStuff.onClick.AddListener(() => ChangeFilter(FilterType.HandStuff));
        buttonHat.onClick.AddListener(() => ChangeFilter(FilterType.Hat));
        buttonConsumable.onClick.AddListener(() => ChangeFilter(FilterType.Consumable));
        buttonWing.onClick.AddListener(() => ChangeFilter(FilterType.Wing));

        //Take off - Wear Register
        buttonTakeoff.onClick.AddListener(() => TakeOff(currentInventorySlotSelected.slotData.ItemInstance));
        buttonWear.onClick.AddListener(() => Wear(currentInventorySlotSelected.slotData.ItemInstance));

        RefreshUI();

        buttonTakeoff.gameObject.SetActive(false);
        buttonWear.gameObject.SetActive(false);

    }
    #region [Filter]
    public void ChangeFilter(FilterType filter)
    {
        currentFilter = filter;
        RefreshUI();
    }
    #endregion

    #region [Outfit Action]
    public void ShowActionButton(ItemInstance ItemToWear)
    {
        if(ItemToWear.ItemStaticData.commonData.itemType != ItemType.Consumable)
        {
            //Case 1: Outfit Equiped
            if (ItemToWear.IsEquiped)
            {
                buttonTakeoff.gameObject.SetActive(true);
                buttonWear.gameObject.SetActive(false);
            }
            else
            {
                buttonWear.gameObject.SetActive(true);
                buttonTakeoff.gameObject.SetActive(false);
            }
            //Case 2: Outfit Unequiped
        }
        else
        {
            //Only Now (Test)
            buttonTakeoff.gameObject.SetActive(false);
            buttonWear.gameObject.SetActive(false);
        }

    }
    public void Wear(ItemInstance ItemToWear)
    {
        OnWear?.Invoke();
        //Luồng: Huỷ IsEquiped - Gán mới tham chiếu 
        if(ItemToWear.ItemStaticData as SO_Outfit)
        {
            Debug.Log("True");
        }
        Debug.Log("Item wear is: " + ItemToWear.ItemStaticData.commonData.itemName);

        //Huỷ Equiped tại Outfit hiện tại
        foreach(InventorySlot inventorySlot in inventoryManager.slots)
        {
                if(inventorySlot.ItemInstance.IsEquiped && inventorySlot.ItemInstance.ItemStaticData.commonData.itemType == ItemToWear.ItemStaticData.commonData.itemType)
                {
                    inventorySlot.ItemInstance.IsEquiped = false;
                }
        }

        if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Shirt)
        {
            inventoryManager.owner.Shirt = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true);
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Hat)
        {
            inventoryManager.owner.Hat = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true); ;
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Glasses)
        {
            inventoryManager.owner.Glasses = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true); ;
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.HandStuff)
        {
            inventoryManager.owner.HandStuff = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true); ;
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Wing)
        {
            inventoryManager.owner.Wing = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true);
            if((ItemToWear.ItemStaticData as SO_Outfit).outfitData.equipClip != null)
            inventoryManager.owner.animator.CrossFadeInFixedTime((ItemToWear.ItemStaticData as SO_Outfit).outfitData.equipClip.name, 0.0f);
        }

        inventoryManager.owner.Wear(ItemToWear.ItemStaticData.commonData.itemType, ItemToWear.ItemStaticData.commonData.itemName);

        ItemToWear.IsEquiped = true;

        ShowActionButton(ItemToWear);

        //Save Data
        SaveSystem.Save(inventoryManager.owner, inventoryManager);
    }
    public void TakeOff(ItemInstance ItemToTakeOff)
    {
        OnTakeOff?.Invoke();

        ItemToTakeOff.IsEquiped = false;

        inventoryManager.owner.TakeOff(ItemToTakeOff.ItemStaticData.commonData.itemType, ItemToTakeOff.ItemStaticData.commonData.itemName);

        ShowActionButton(ItemToTakeOff);

        //Save Data
        SaveSystem.Save(inventoryManager.owner, inventoryManager);
    }
    #endregion

    public void RefreshUI()
    {
        // Xoá hết UI Slot cũ
        foreach (var slot in uiSlots)
        {
            pooler.Free(slot);
        }
        uiSlots.Clear();

        // Lọc dữ liệu inventory theo filter
        var filteredSlots = inventoryManager.slots.Where(slot =>
            (currentFilter == FilterType.Shirt && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Shirt) ||
            (currentFilter == FilterType.Glasses && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Glasses) ||
            (currentFilter == FilterType.HandStuff && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.HandStuff) ||
            (currentFilter == FilterType.Hat && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Hat) ||
            (currentFilter == FilterType.Consumable && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Consumable) ||
            (currentFilter == FilterType.Wing && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Wing) 
        );

        // Tạo UI Slot mới dựa trên dữ liệu đã lọc
        foreach (var invSlot in filteredSlots)
        {
            UIInventorySlot uiSlot = pooler.GetNew();
            uiSlot.slotData = invSlot;

            uiSlot.SetupSlot(invSlot);
            uiSlots.Add(uiSlot);
        }

        UpdateResourceUI();
    }

    public void UpdateResourceUI()
    {
        Energy.text = inventoryManager.owner.CurrentEnergy.ToString() + " / " + inventoryManager.owner.MaxEnergy.ToString();
        Coin.text = inventoryManager.owner.Coin.ToString();
    }
}
