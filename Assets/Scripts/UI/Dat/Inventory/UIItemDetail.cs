using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetail : MonoBehaviour
{
    [Header("UI Informations")]
    public Image Image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Descriptions;

    [Header("Buttons")]
    public Button Use_Button;
    public Button Sell_Button;
    public Button Buy_Button;

    [Header("References")]
    public UIInventory UIInventory;
    public UIAmountPicker UIMountPicker;
    public ItemInstance CurrentItemInstance;

    private void Awake()
    {
        if (Use_Button != null) Use_Button.onClick.AddListener(() => Use());
        if (Sell_Button != null) Sell_Button.onClick.AddListener(() => Sell());
        //if (Buy_Button != null) Buy_Button.onClick.AddListener(() => Buy());
    }
    
    public void SetupItemDetail(ItemInstance ItemInstance)
    {
        CurrentItemInstance = ItemInstance;

        Image.sprite = CurrentItemInstance.ItemStaticData.commonData.icon;
        Name.text = CurrentItemInstance.ItemStaticData.commonData.itemName;
        Descriptions.text = CurrentItemInstance.ItemStaticData.commonData.description;
    }

    public void Use()
    {
        UIMountPicker.gameObject.SetActive(true);
        UIMountPicker.SetupAmountPicker(CurrentItemInstance, EPickerAction.Use);
    }

    public void Sell()
    {
        UIMountPicker.gameObject.SetActive(true);
        UIMountPicker.SetupAmountPicker(CurrentItemInstance, EPickerAction.Sell);
    }

    public void Buy()
    {
        UIMountPicker.gameObject.SetActive(true);
        UIMountPicker.SetupAmountPicker(CurrentItemInstance, EPickerAction.Buy);
    }
}
