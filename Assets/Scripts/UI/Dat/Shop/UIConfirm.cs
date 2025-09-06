using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirm : MonoBehaviour
{
    [Header("Item Information Holding")]
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Price;

    [Header("Button")]
    public Button Buy_Button;

    [Header("Reference")]
    public UIShop UIShop;

    private void Awake()
    {
        Buy_Button.onClick.AddListener(() => Buy(UIShop.CurrentUIShopSlotSelected.item));
    }
    public void SetupItemInfo(SO_Item item)
    {
        Icon.sprite = item.commonData.icon;
        Name.text = item.name;
        Description.text = item.commonData.description;
        Price.text = item.commonData.price.ToString();
    }
    public void Buy(SO_Item item)
    {
        UIShop.ShopManager.Inventory.AddItem(new ItemInstance(item, 1, false));
        gameObject.SetActive(false);
    }
}
