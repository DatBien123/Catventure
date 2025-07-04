using System;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    [Header("Shop Items UI")]
    public GameObject shopSlotPrefab;
    public Transform shopContentParent;
    public UIShopItemInfo uiShopItemInfo;
    public List<UIShopSlot> uiShopSlots = new List<UIShopSlot>();
    public Shop shopManager;

    [Header("Shop Item Types")]
    public GameObject itemTypePrefab;
    public Transform itemTypeParent;
    public List<UIShopItemType> shopItemTypes = new List<UIShopItemType>();
    //[HideInInspector]
    public UIShopItemType currentShopItemType;

    [Header("Shop Item Specific Types")]
    public GameObject itemSpecificTypePrefab;
    public Transform itemSpecificTypeParent;
    public List<UIShopItemTypeSpecific> shopItemTypeSpecifics = new List<UIShopItemTypeSpecific>();
    //[HideInInspector]
    public UIShopItemTypeSpecific currentShopItemSpecificType;

    public UIShopSlot currentUIShopSlotSelected;

    void Start()
    {
        InitTemp();

        RefreshShopUI();
    }
    public void InitTemp()
    {
        //Init teamp
        currentShopItemType = new UIShopItemType();
        currentShopItemType.itemType = ItemType.Outfit;

        currentShopItemSpecificType = new UIShopItemTypeSpecific();
        currentShopItemSpecificType.outfitType = EOutfitType.Hat;
        currentShopItemSpecificType.consumableType = EConsumableType.Food;
    }
    public void RefreshShopUI()
    {

        // Xoá hết Filter  cũ
        foreach (var shopItemType in shopItemTypes)
        {
            Destroy(shopItemType.gameObject);
        }
        shopItemTypes.Clear();

        // Xoá hết Filter cụ thể cũ
        foreach (var shopItemTypeSpecific in shopItemTypeSpecifics)
        {
            Destroy(shopItemTypeSpecific.gameObject);
        }
        shopItemTypeSpecifics.Clear();


        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            GameObject itemTypeObj = Instantiate(itemTypePrefab, itemTypeParent);
            UIShopItemType shopItemType = itemTypeObj.GetComponent<UIShopItemType>();
            shopItemType.SetUp(type);
            shopItemTypes.Add(shopItemType);
        }

        if(currentShopItemType.itemType == ItemType.Outfit)
        {
            foreach (EOutfitType outfitType in Enum.GetValues(typeof(EOutfitType)))
            {
                if (outfitType == EOutfitType.None) continue;
                GameObject itemTypeObj = Instantiate(itemSpecificTypePrefab, itemSpecificTypeParent);
                UIShopItemTypeSpecific shopItemTypeSpecific = itemTypeObj.GetComponent<UIShopItemTypeSpecific>();
                shopItemTypeSpecific.SetUp(EConsumableType.None, outfitType);
                shopItemTypeSpecifics.Add(shopItemTypeSpecific);
            }
        }
        else if(currentShopItemType.itemType == ItemType.Consumable)
        {
            foreach (EConsumableType consumableType in Enum.GetValues(typeof(EConsumableType)))
            {
                if (consumableType == EConsumableType.None) continue;
                GameObject itemTypeObj = Instantiate(itemSpecificTypePrefab, itemSpecificTypeParent);
                UIShopItemTypeSpecific shopItemTypeSpecific = itemTypeObj.GetComponent<UIShopItemTypeSpecific>();
                shopItemTypeSpecific.SetUp(consumableType, EOutfitType.None);
                shopItemTypeSpecifics.Add(shopItemTypeSpecific);
            }
        }

        // Lọc dữ liệu inventory theo filter
        List<SO_Item> filteredShopItems = new List<SO_Item>();
        foreach(var shopItem in shopManager.shopItems)
        {
            if( shopItem.commonData.itemType == currentShopItemType.itemType)
            {
                if(shopItem as SO_Consumable)
                {
                    if((shopItem as SO_Consumable).ConsumableData.ConsumableType == currentShopItemSpecificType.consumableType)
                    {
                        filteredShopItems.Add(shopItem as SO_Consumable);
                        continue;
                    }
                }
                else if (shopItem as SO_Outfit)
                {
                    if ((shopItem as SO_Outfit).outfitData.Type == currentShopItemSpecificType.outfitType)
                    {
                        filteredShopItems.Add(shopItem as SO_Outfit);
                        continue;
                    }
                }

            }
        }

        // Xoá hết UI Shop Slot cũ
        foreach (var slot in uiShopSlots)
        {
            Destroy(slot.gameObject);
        }
        uiShopSlots.Clear();

        //Xóa cả những "con" của shopContentParent
        foreach (Transform child in shopContentParent)
            Destroy(child.gameObject);

        foreach (var shopItem in filteredShopItems)
        {
            GameObject slotObj = Instantiate(shopSlotPrefab, shopContentParent);
            UIShopSlot slot = slotObj.GetComponent<UIShopSlot>();
            slot.Setup(shopItem, shopManager);
            uiShopSlots.Add(slot);
        }
    }

    public void RefreshByShopItemType()
    {
        // Xoá hết Filter cụ thể cũ
        foreach (var shopItemTypeSpecific in shopItemTypeSpecifics)
        {
            Destroy(shopItemTypeSpecific.gameObject);
        }
        shopItemTypeSpecifics.Clear();

        if (currentShopItemType.itemType == ItemType.Outfit)
        {
            foreach (EOutfitType outfitType in Enum.GetValues(typeof(EOutfitType)))
            {
                if (outfitType == EOutfitType.None) continue;
                GameObject itemTypeObj = Instantiate(itemSpecificTypePrefab, itemSpecificTypeParent);
                UIShopItemTypeSpecific shopItemTypeSpecific = itemTypeObj.GetComponent<UIShopItemTypeSpecific>();
                shopItemTypeSpecific.SetUp(EConsumableType.None, outfitType);
                shopItemTypeSpecifics.Add(shopItemTypeSpecific);
            }
        }
        else if (currentShopItemType.itemType == ItemType.Consumable)
        {
            foreach (EConsumableType consumableType in Enum.GetValues(typeof(EConsumableType)))
            {
                if (consumableType == EConsumableType.None) continue;
                GameObject itemTypeObj = Instantiate(itemSpecificTypePrefab, itemSpecificTypeParent);
                UIShopItemTypeSpecific shopItemTypeSpecific = itemTypeObj.GetComponent<UIShopItemTypeSpecific>();
                shopItemTypeSpecific.SetUp(consumableType, EOutfitType.None);
                shopItemTypeSpecifics.Add(shopItemTypeSpecific);
            }
        }

        // Lọc dữ liệu inventory theo filter
        List<SO_Item> filteredShopItems = new List<SO_Item>();
        foreach (var shopItem in shopManager.shopItems)
        {
            if (shopItem.commonData.itemType == currentShopItemType.itemType)
            {
                if (shopItem as SO_Consumable)
                {
                    if ((shopItem as SO_Consumable).ConsumableData.ConsumableType == currentShopItemSpecificType.consumableType)
                    {
                        filteredShopItems.Add(shopItem as SO_Consumable);
                        continue;
                    }
                }
                else if (shopItem as SO_Outfit)
                {
                    if ((shopItem as SO_Outfit).outfitData.Type == currentShopItemSpecificType.outfitType)
                    {
                        filteredShopItems.Add(shopItem as SO_Outfit);
                        continue;
                    }
                }

            }
        }

        // Xoá hết UI Shop Slot cũ
        foreach (var slot in uiShopSlots)
        {
            Destroy(slot.gameObject);
        }
        uiShopSlots.Clear();

        //Xóa cả những "con" của shopContentParent
        foreach (Transform child in shopContentParent)
            Destroy(child.gameObject);

        foreach (var shopItem in filteredShopItems)
        {
            GameObject slotObj = Instantiate(shopSlotPrefab, shopContentParent);
            UIShopSlot slot = slotObj.GetComponent<UIShopSlot>();
            slot.Setup(shopItem, shopManager);
            uiShopSlots.Add(slot);
        }

    }

    public void RefreshByShopItemTypeSpecific()
    {

        // Lọc dữ liệu inventory theo filter
        List<SO_Item> filteredShopItems = new List<SO_Item>();
        foreach (var shopItem in shopManager.shopItems)
        {
            if (shopItem.commonData.itemType == currentShopItemType.itemType)
            {
                if (shopItem as SO_Consumable)
                {
                    if ((shopItem as SO_Consumable).ConsumableData.ConsumableType == currentShopItemSpecificType.consumableType)
                    {
                        filteredShopItems.Add(shopItem as SO_Consumable);
                        continue;
                    }
                }
                else if (shopItem as SO_Outfit)
                {
                    if ((shopItem as SO_Outfit).outfitData.Type == currentShopItemSpecificType.outfitType)
                    {
                        filteredShopItems.Add(shopItem as SO_Outfit);
                        continue;
                    }
                }

            }
        }

        // Xoá hết UI Shop Slot cũ
        foreach (var slot in uiShopSlots)
        {
            Destroy(slot.gameObject);
        }
        uiShopSlots.Clear();

        //Xóa cả những "con" của shopContentParent
        foreach (Transform child in shopContentParent)
            Destroy(child.gameObject);

        foreach (var shopItem in filteredShopItems)
        {
            GameObject slotObj = Instantiate(shopSlotPrefab, shopContentParent);
            UIShopSlot slot = slotObj.GetComponent<UIShopSlot>();
            slot.Setup(shopItem, shopManager);
            uiShopSlots.Add(slot);
        }
    }
}
