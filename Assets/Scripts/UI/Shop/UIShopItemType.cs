using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItemType : MonoBehaviour, IPointerDownHandler
{
    public ItemType itemType;
    public Image itemImage;
    public Image backgroundImage;
    public TextMeshProUGUI textMeshProUGUI;
    public bool isSelected;

    [Header("Reference")]
    public UIShop uiShop;

    private void Awake()
    {
        uiShop = GameObject.FindAnyObjectByType<UIShop>();
    }

    public void ResetShopDisplay()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var shopItemType in uiShop.shopItemTypes)
        {
            shopItemType.isSelected = false;
            shopItemType.OnSelect(false);
        }

        isSelected = true;
        uiShop.currentShopItemType = this;
        OnSelect(true);
        // Xoá hết Filter cụ thể cũ
        foreach (var shopItemTypeSpecific in uiShop.shopItemTypeSpecifics)
        {
            Destroy(shopItemTypeSpecific.gameObject);
        }
        uiShop.shopItemTypeSpecifics.Clear();

        if (uiShop.currentShopItemType.itemType == ItemType.Outfit)
        {
            foreach (EOutfitType outfitType in Enum.GetValues(typeof(EOutfitType)))
            {
                if (outfitType == EOutfitType.None) continue;
                GameObject itemTypeObj = Instantiate(uiShop.itemSpecificTypePrefab, uiShop.itemSpecificTypeParent);
                UIShopItemTypeSpecific shopItemTypeSpecific = itemTypeObj.GetComponent<UIShopItemTypeSpecific>();
                shopItemTypeSpecific.SetUp(EConsumableType.None, outfitType);
                uiShop.shopItemTypeSpecifics.Add(shopItemTypeSpecific);
            }
        }
        else if (uiShop.currentShopItemType.itemType == ItemType.Consumable)
        {
            foreach (EConsumableType consumableType in Enum.GetValues(typeof(EConsumableType)))
            {
                if (consumableType == EConsumableType.None) continue;
                GameObject itemTypeObj = Instantiate(uiShop.itemSpecificTypePrefab, uiShop.itemSpecificTypeParent);
                UIShopItemTypeSpecific shopItemTypeSpecific = itemTypeObj.GetComponent<UIShopItemTypeSpecific>();
                shopItemTypeSpecific.SetUp(consumableType, EOutfitType.None);
                uiShop.shopItemTypeSpecifics.Add(shopItemTypeSpecific);
            }
        }
        uiShop.currentShopItemSpecificType = uiShop.shopItemTypeSpecifics[0];

        uiShop.RefreshByShopItemType();
    }

    public void SetUp(ItemType type)
    {
        itemType = type;

        SO_ButtonData buttonData = Resources.Load<SO_ButtonData>($"Graphics/ItemType/{itemType}");
        itemImage.sprite = buttonData.buttonData.mainIcon;

        textMeshProUGUI.text = itemType.ToString();
    }

    public void OnSelect(bool isSelect)
    {

        SO_ButtonData buttonData = Resources.Load<SO_ButtonData>($"Graphics/ItemType/{itemType}");
        if (isSelect)
        {
            backgroundImage.sprite = buttonData.buttonData.selectFieldData.backgroundSprite;
            backgroundImage.color = buttonData.buttonData.selectFieldData.backgroundColor;
            textMeshProUGUI.color = buttonData.buttonData.selectFieldData.textColor;
        }
        else
        {
            backgroundImage.sprite = buttonData.buttonData.deselectdFieldData.backgroundSprite;
            backgroundImage.color = buttonData.buttonData.deselectdFieldData.backgroundColor;
            textMeshProUGUI.color = buttonData.buttonData.deselectdFieldData.textColor;
        }
    }
}