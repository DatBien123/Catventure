using System;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    [Header("Shop Items UI")]
    public UIShopSlot ShopSlotPrefab;
    public Transform ShopContentParent;
    public List<UIShopSlot> uiShopSlots = new List<UIShopSlot>();

    [Header("Shop Item Types")]
    public GameObject itemTypePrefab;
    public Transform itemTypeParent;

    [Header("Reference")]
    public UIShopSlot CurrentUIShopSlotSelected;
    public Shop ShopManager;
    public UIShopItemInfo UiShopItemInfo;

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
        RefreshShopUI();
    }
    public void RefreshShopUI()
    {

        
    }
}
