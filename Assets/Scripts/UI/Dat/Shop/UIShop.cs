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
    public Button buttonConsumable;

    [Header("Reference")]
    public UIInventory UIInventory;
    public UIYabis UIYabis;
    public UIConfirm UIConfirmPurchase;

    [Header("Data")]
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Coin;
    public UIShopSlot CurrentUIShopSlotSelected;
    public List<UIShopSlot> uiShopSlots = new List<UIShopSlot>();
    private FilterType currentFilter = FilterType.Shirt;

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
        buttonShirt.onClick.AddListener(() => ChangeFilter(FilterType.Shirt));
        buttonGlasses.onClick.AddListener(() => ChangeFilter(FilterType.Glasses));
        buttonHandStuff.onClick.AddListener(() => ChangeFilter(FilterType.HandStuff));
        buttonHat.onClick.AddListener(() => ChangeFilter(FilterType.Hat));
        buttonConsumable.onClick.AddListener(() => ChangeFilter(FilterType.Consumable));

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
            (currentFilter == FilterType.Consumable && slot.commonData.itemType == ItemType.Consumable)
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
