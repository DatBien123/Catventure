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

    [Header("Success Popup")]
    public GameObject Success_Popup;
    public Image Success_Icon;
    public TextMeshProUGUI Success_Name;

    [Header("Fail Popup")]
    public GameObject Fail_Popup;
    public Image Fail_Icon;
    public TextMeshProUGUI Fail_Price;

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

        Success_Icon.sprite = item.commonData.icon;
        Success_Name.text = item.name;

        Fail_Icon.sprite = item.commonData.icon;
        Fail_Price.text = Mathf.Abs(item.commonData.price - UIShop.ShopManager.owner.Coin).ToString();
    }
    public void Buy(SO_Item item)
    {
        if(UIShop.ShopManager.owner.Coin >= item.commonData.price)
        {
            //Hien thi ui tuong ung
            UIShop.ShopManager.Inventory.AddItem(new ItemInstance(item, 1, false));
            Success_Popup.gameObject.SetActive(true);
            gameObject.SetActive(false);

            //Data
            UIShop.ShopManager.owner.Coin -= item.commonData.price;

            //Update Resource data cho cac giao dien khac
            UIShop.UpdateResourceUI();
            UIShop.UIYabis.UpdateResourceUI();
            UIShop.UIInventory.UpdateResourceUI();

            SaveSystem.Save(UIShop.ShopManager.owner, UIShop.UIInventory.inventoryManager);
        }
        else
        {
            Fail_Popup.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
