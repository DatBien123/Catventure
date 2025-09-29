using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIShopSlot : MonoBehaviour,IObjectPool<UIShopSlot>, IPointerClickHandler
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
        priceText.text = item.commonData.price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiShop.AudioManager != null)
        {
            uiShop.AudioManager.PlaySFX("Choose Item");
        }

        if (uiShop.TutorialManager.currentPart.TutorialName == "Farm Tutorial" && uiShop.TutorialManager.currentStep.stepName == "Rời khỏi Cửa hàng Hạt giống") return;
            //Loop through all these slots of inventory
            //Deselect All
            foreach (var uiShop in uiShop.uiShopSlots)
        {
            uiShop.isSelected = false;
        }

        uiShop.CurrentUIShopSlotSelected = this;
        isSelected = true;

        uiShop.UIConfirmPurchase.SetupItemInfo(item);
        uiShop.UIConfirmPurchase.gameObject.SetActive(true);
        uiShop.UIItemDetail.SetupItemDetail(new ItemInstance(item, 1));

        //ShopManager.Inventory.AddItem(new ItemInstance(item, 1, false));

        Debug.Log("Show Item Confirm: " + item.name);

        if(uiShop.TutorialManager.currentPart.TutorialName == "Farm Tutorial" && uiShop.TutorialManager.currentStep.stepName == "Chọn một Hạt giống bất kì")
        {
            uiShop.TutorialManager.ApplyNextStep("Chọn một Hạt giống bất kì");
        }
    }
}