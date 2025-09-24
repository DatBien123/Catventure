using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum EPickerAction
{
    None = 0,
    Sell,
    Buy,
    Use
}
public class UIAmountPicker : MonoBehaviour
{
    [Header("UI Informations")]
    public Image Image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Quantity;
    public TextMeshProUGUI TotalMoney;

    [Header("Buttons")]
    public Button Decrease_Button;
    public Button Increase_Button;

    public Button Ok_Button;


    [Header("References")]
    public UIItemDetail UIItemDetail;

    [Header("Data")]
    public EPickerAction CurrentPickerAction;

    public int quantity;
    public int totalQuantity;
    public float totalMoney;
    public void Awake()
    {
        if (Decrease_Button != null) Decrease_Button.onClick.AddListener(() => Decrease());
        if (Increase_Button != null) Increase_Button.onClick.AddListener(() => Increase());
        if (Ok_Button != null) Ok_Button.onClick.AddListener(() => Apply());
    }

    public void SetupAmountPicker(ItemInstance ItemInstance, EPickerAction PickerAction)
    {
        CurrentPickerAction = PickerAction;

        //Setup Data
        quantity = 1;

        if (CurrentPickerAction == EPickerAction.Use)
        {
            totalQuantity = ItemInstance.Quantity;
            TotalMoney.gameObject.SetActive(false);
        }
        else if (CurrentPickerAction == EPickerAction.Buy)
        {
            totalQuantity =
            (int)(UIItemDetail.UIInventory.inventoryManager.owner.Coin / ItemInstance.ItemStaticData.commonData.price);
            TotalMoney.gameObject.SetActive(true);
        }
        else if (CurrentPickerAction == EPickerAction.Sell)
        {
            totalQuantity = ItemInstance.Quantity;
            TotalMoney.gameObject.SetActive(true);
        }
        //

        //Setup UI
        Image.sprite = ItemInstance.ItemStaticData.commonData.icon;
        Name.text = ItemInstance.ItemStaticData.commonData.itemName;
        Quantity.text = quantity.ToString() + " / " + totalQuantity.ToString();

        if(CurrentPickerAction == EPickerAction.Sell) TotalMoney.text = "Sell Price: " + (ItemInstance.ItemStaticData.commonData.sellPrice * quantity).ToString();
        else if(CurrentPickerAction == EPickerAction.Buy) TotalMoney.text = "Total Price: " + (ItemInstance.ItemStaticData.commonData.sellPrice * quantity).ToString();

    }

    #region [Actions]
    public void Decrease()
    {
        quantity -= 1;
        if(quantity < 1)
        {
            quantity = totalQuantity;
        }

        if(CurrentPickerAction == EPickerAction.Sell) totalMoney = quantity * UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.sellPrice;
        else if(CurrentPickerAction == EPickerAction.Buy ) totalMoney = quantity * UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.price;

        UpdateAmountPicker();
    }
    public void Increase()
    {
        quantity += 1;
        if (quantity > totalQuantity)
        {
            quantity = 1;
        }

        if (CurrentPickerAction == EPickerAction.Sell) totalMoney = quantity * UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.sellPrice;
        else if (CurrentPickerAction == EPickerAction.Buy) totalMoney = quantity * UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.price;

        UpdateAmountPicker();
    }
    public void Apply()
    {
        if (CurrentPickerAction == EPickerAction.Sell)
        {
            UIItemDetail.UIInventory.inventoryManager.owner.Coin += UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.sellPrice * quantity;
            UIItemDetail.UIInventory.inventoryManager.RemoveItem(UIItemDetail.CurrentItemInstance.ItemStaticData, quantity);

            UIItemDetail.UIInventory.UIYabis.UpdateResourceUI();
            UIItemDetail.UIInventory.RefreshUI();

        }
        else if( CurrentPickerAction == EPickerAction.Buy)
        {
            UIItemDetail.UIInventory.inventoryManager.owner.Coin -= UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.price * quantity;
            UIItemDetail.UIInventory.inventoryManager.AddItem( new ItemInstance(UIItemDetail.CurrentItemInstance.ItemStaticData, quantity, false));

            UIItemDetail.UIInventory.UIYabis.UpdateResourceUI();
        }
        else if(CurrentPickerAction == EPickerAction.Use)
        {
            UIItemDetail.UIInventory.inventoryManager.owner.CurrentEnergy += (UIItemDetail.CurrentItemInstance.ItemStaticData as SO_Consumable).ConsumableData.energyRestore* quantity;

            if (UIItemDetail.UIInventory.inventoryManager.owner.CurrentEnergy >= UIItemDetail.UIInventory.inventoryManager.owner.MaxEnergy) UIItemDetail.UIInventory.inventoryManager.owner.CurrentEnergy = UIItemDetail.UIInventory.inventoryManager.owner.MaxEnergy;
            UIItemDetail.UIInventory.inventoryManager.RemoveItem(UIItemDetail.CurrentItemInstance.ItemStaticData, quantity);

            UIItemDetail.UIInventory.UIYabis.UpdateResourceUI();
            UIItemDetail.UIInventory.RefreshUI();
        }

        UIItemDetail.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    public void UpdateAmountPicker()
    {
        Quantity.text = quantity.ToString() + " / " + totalQuantity.ToString();
        TotalMoney.text = (UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.price * quantity).ToString();

        if (CurrentPickerAction == EPickerAction.Sell) TotalMoney.text = "Sell Price: " + (UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.sellPrice * quantity).ToString();
        else if (CurrentPickerAction == EPickerAction.Buy) TotalMoney.text = "Total Price: " + (UIItemDetail.CurrentItemInstance.ItemStaticData.commonData.sellPrice * quantity).ToString();
    }
}
