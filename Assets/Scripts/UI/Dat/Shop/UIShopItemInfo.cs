using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIShopItemInfo : MonoBehaviour
{
    [Header("UI Elements")]
    public UnityEngine.UI.Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Price;
    public TextField TextField;

    [Header("On Interaction")]
    public UnityEngine.UI.Button BuyButton;
    public UnityEngine.UI.Button CancelButton;

    [Header("References")]
    public UIShop UIShop;
    private void Start()
    {
        //buyButton.onClick.AddListener(() => uiShop.shopManager.TryBuy(uiShop.currentUIShopSlotSelected.item));
    }

    public void ShowInfo(SO_Item item)
    {
        this.gameObject.SetActive(true);
        Icon.sprite = item.commonData.icon;
        Icon.enabled = true;

        Name.text = item.commonData.itemName;
        Price.text = item.commonData.price.ToString();
    }
    public void HideInfo()
    {
        this.gameObject.SetActive(false);
        Icon.sprite = null;
        Icon.enabled = false;

        Name.text = "";
        Price.text = "";
    }
}
