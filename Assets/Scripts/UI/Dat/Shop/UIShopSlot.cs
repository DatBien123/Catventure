using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIShopSlot : MonoBehaviour,IObjectPool<UIShopSlot>, IPointerDownHandler
{
    [Header("Item Infomations")]
    public Image itemIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public SO_Item item;

    [Header("On Interact")]
    public bool isSelected;

    [Header("References")]
    public UIShop uiShop;
    public Shop ShopManager;


    public int poolID { get; set; }
    public ObjectPooler<UIShopSlot> pool { get; set; }

    private void Awake()
    {
        uiShop = GameObject.FindAnyObjectByType<UIShop>();
        ShopManager = GameObject.FindAnyObjectByType<Shop>();
    }
    public void Setup(SO_Item shopItem)
    {
        item = shopItem;
         
        itemIcon.sprite = item.commonData.icon;
        nameText.text = item.commonData.itemName;
        nameText.color = item.displayData.buttonData.deselectdFieldData.textColor;
        priceText.text = item.commonData.price.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Loop through all these slots of inventory
        //Deselect All
        foreach (var uiShop in uiShop.uiShopSlots)
        {
            uiShop.isSelected = false;
        }

        uiShop.CurrentUIShopSlotSelected = this;
        isSelected = true;

        ShopManager.Inventory.AddItem(new ItemInstance(item, 1, false));

    }
}