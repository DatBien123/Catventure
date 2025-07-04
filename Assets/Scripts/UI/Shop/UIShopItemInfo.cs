using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIShopItemInfo : MonoBehaviour
{
    [Header("UI Elements")]
    public UnityEngine.UI.Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextField TextField;

    [Header("On Interaction")]
    public UnityEngine.UI.Button buyButton;
    public UnityEngine.UI.Button previewButton;

    [Header("References")]
    public UIShop uiShop;
    private void Start()
    {
        buyButton.onClick.AddListener(() => uiShop.shopManager.TryBuy(uiShop.currentUIShopSlotSelected.item));
    }

    public void ShowInfo(SO_Item item)
    {
        this.gameObject.SetActive(true);
        itemIcon.sprite = item.commonData.icon;
        itemIcon.enabled = true;

        itemName.text = item.commonData.itemName;
        itemPrice.text = item.commonData.price.ToString();
    }
    public void HideInfo()
    {
        this.gameObject.SetActive(false);
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        itemName.text = "";
        itemPrice.text = "";
    }
}
