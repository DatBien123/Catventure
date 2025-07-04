using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIShopSlot : MonoBehaviour, IPointerDownHandler
{
    [Header("Item Infomations")]
    public Image itemIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public SO_Item item;

    [Header("Item Infomations")]
    public Button buyButton;

    [Header("On Interact")]
    public Image backGroundImage;
    public bool isSelected;

    [Header("References")]
    public UIShop uiShop;
    private Shop shopManager;

    private void Awake()
    {
        uiShop = GameObject.FindAnyObjectByType<UIShop>();
    }
    public void Setup(SO_Item shopItem, Shop manager)
    {
        item = shopItem;
        shopManager = manager;

        OnSelected(false);
         
        itemIcon.sprite = item.commonData.icon;
        nameText.text = item.commonData.itemName;
        nameText.color = item.displayData.buttonData.deselectdFieldData.textColor;
        priceText.text = item.commonData.price.ToString();

        buyButton.onClick.AddListener(() =>
        {
            if (isSelected)
            {
                uiShop.uiShopItemInfo.gameObject.SetActive(true);
                uiShop.uiShopItemInfo.ShowInfo(uiShop.currentUIShopSlotSelected.item);
                //shopManager.TryBuy(item);
            }
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Loop through all these slots of inventory
        //Deselect All
        foreach (var uiShop in uiShop.uiShopSlots)
        {
            uiShop.isSelected = false;
            uiShop.OnSelected(false);
        }

        uiShop.currentUIShopSlotSelected = this;
        isSelected = true;
        OnSelected(true);

    }

    public void OnSelected(bool isSelected)
    {
        if (isSelected)
        {
            backGroundImage.sprite = item.displayData.buttonData.selectFieldData.backgroundSprite;
            backGroundImage.color = item.displayData.buttonData.selectFieldData.backgroundColor;
        }
        else
        {
            backGroundImage.sprite = item.displayData.buttonData.deselectdFieldData.backgroundSprite;
            backGroundImage.color = item.displayData.buttonData.deselectdFieldData.backgroundColor;
        }
    }
}