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
        Name.text = item.commonData.itemName;
        Description.text = item.commonData.description;
        Price.text = item.commonData.price.ToString();

        Success_Icon.sprite = item.commonData.icon;
        Success_Name.text = item.commonData.itemName;

        Fail_Icon.sprite = item.commonData.icon;
        Fail_Price.text = Mathf.Abs(item.commonData.price - UIShop.ShopManager.owner.Coin).ToString();

    }
    public void Buy(SO_Item item)
    {


        if(item is SO_Outfit)
        {
            if (UIShop.ShopManager.owner.Coin >= item.commonData.price)
            {
                if (UIShop.AudioManager != null)
                {
                    UIShop.AudioManager.PlaySFX("Buy");
                }

                //UI
                Success_Popup.gameObject.SetActive(true);
                gameObject.SetActive(false);

                //Data
                UIShop.ShopManager.TryBuy(new ItemInstance(item, 1, false));

                //Update Resource data cho cac giao dien khac
                if (UIShop.UIYabis)
                    UIShop.UIYabis.UpdateResourceUI();

                UIShop.RefreshShopUI();
            }
            else
            {
                Fail_Popup.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
            Debug.Log("Item is Outfit");
        }
        else
        {
            if (UIShop.AudioManager != null)
            {
                UIShop.AudioManager.PlaySFX("Apply");
            }

            if (UIShop.ShopManager.owner.Coin >= item.commonData.price)
            {
                Debug.Log("Item is not Outfit");
                UIShop.UIItemDetail.UIMountPicker.gameObject.SetActive(true);
                UIShop.UIItemDetail.UIMountPicker.SetupAmountPicker(new ItemInstance(item, 1), EPickerAction.Buy);

                if (UIShop.TutorialManager.currentPart.TutorialName == "Farm Tutorial" && UIShop.TutorialManager.currentStep.stepName == "Mua hạt giống")
                {
                    UIShop.TutorialManager.ApplyNextStep("Mua hạt giống");
                }
            }
            else
            {
                Fail_Popup.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
