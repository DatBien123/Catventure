using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public Shop ShopManager;

    [Header("Shop Items UI")]
    public UIShopSlot ShopSlotPrefab;
    public Transform ShopContentParent;

    [Header("Filter Buttons")]
    public Button buttonShirt;
    public Button buttonHandStuff;
    public Button buttonGlasses;
    public Button buttonHat;
    public Button buttonWing;
    public Button buttonConsumable;
    public Button buttonCrops;

    [Header("Reference")]
    public UIInventory UIInventory;
    public UIItemDetail UIItemDetail;
    public UIYabis UIYabis;
    public UIConfirm UIConfirmPurchase;

    [Header("Data")]
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Coin;
    public UIShopSlot CurrentUIShopSlotSelected;
    public List<UIShopSlot> uiShopSlots = new List<UIShopSlot>();
    public FilterType currentFilter = FilterType.Shirt;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 100;
    protected ObjectPooler<UIShopSlot> pooler { get; private set; }
    //protected GameObject poolParent;

    #endregion

    private void Awake()
    {
        pooler = new ObjectPooler<UIShopSlot>();
        pooler.Initialize(this, poolCount, ShopSlotPrefab, ShopContentParent);
    }
    void Start()
    {
        //Filter Register
        if(buttonShirt != null) buttonShirt.onClick.AddListener(() => ChangeFilter(FilterType.Shirt));
        if (buttonGlasses != null) buttonGlasses.onClick.AddListener(() => ChangeFilter(FilterType.Glasses));
        if (buttonHandStuff != null) buttonHandStuff.onClick.AddListener(() => ChangeFilter(FilterType.HandStuff));
        if (buttonHat != null) buttonHat.onClick.AddListener(() => ChangeFilter(FilterType.Hat));
        if (buttonConsumable != null) buttonConsumable.onClick.AddListener(() => ChangeFilter(FilterType.Consumable));
        if (buttonWing != null) buttonWing.onClick.AddListener(() => ChangeFilter(FilterType.Wing));
        if (buttonCrops != null) buttonCrops.onClick.AddListener(() => ChangeFilter(FilterType.Crops));

        RefreshShopUI();
    }
    public void ChangeFilter(FilterType filter)
    {
        currentFilter = filter;
        RefreshShopUI();
    }
    public void RefreshShopUI()
    {
        // Xoá hết UI Slot cũ
        foreach (var slot in uiShopSlots)
        {
            pooler.Free(slot);
        }
        uiShopSlots.Clear();

        // Lọc dữ liệu inventory theo filter
        var filteredSlots = ShopManager.slots.Where(slot =>
            (currentFilter == FilterType.Shirt && slot.commonData.itemType == ItemType.Shirt) ||
            (currentFilter == FilterType.Glasses && slot.commonData.itemType == ItemType.Glasses) ||
            (currentFilter == FilterType.HandStuff && slot.commonData.itemType == ItemType.HandStuff) ||
            (currentFilter == FilterType.Hat && slot.commonData.itemType == ItemType.Hat) ||
            (currentFilter == FilterType.Consumable && slot.commonData.itemType == ItemType.Consumable) ||
            (currentFilter == FilterType.Wing && slot.commonData.itemType == ItemType.Wing) ||
            (currentFilter == FilterType.Crops && slot.commonData.itemType == ItemType.Crops)
        );

        // Tạo UI Slot mới dựa trên dữ liệu đã lọc
        foreach (var invSlot in filteredSlots)
        {
            UIShopSlot uiSlot = pooler.GetNew();
            uiSlot.item = invSlot;

            uiSlot.Setup(invSlot);
            uiShopSlots.Add(uiSlot);
        }

        UpdateResourceUI();

    }

    public void UpdateResourceUI()
    {
        Energy.text = ShopManager.owner.CurrentEnergy.ToString() + " / " + ShopManager.owner.MaxEnergy.ToString();
        Coin.text = ShopManager.owner.Coin.ToString();
    }
}
