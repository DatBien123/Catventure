using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetail : MonoBehaviour
{
    [Header("UI Informations")]
    public Image Image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Descriptions;
    public TextMeshProUGUI Enegy;
    public TextMeshProUGUI SellPrice;

    [Header("Buttons")]
    public Button Use_Button;
    public Button Sell_Button;
    public Button Buy_Button;

    [Header("References")]
    public UIInventory UIInventory;
    public UIAmountPicker UIMountPicker;
    public ItemInstance CurrentItemInstance;
    public GameObject ItemDetail;
    private void OnEnable()
    {
        ItemDetail.transform.localScale = Vector3.zero; // Start from zero scale
        ItemDetail.transform.DOScale(1.1f, 0.3f) // Zoom to 1.1f slightly overshooting
            .SetEase(Ease.OutBack) // Adds a back effect for the overshoot
            .OnComplete(() => ItemDetail.transform.DOScale(1f, 0.2f)); // Settle back to 1f
    }
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

        if(Enegy != null)
        Enegy.text = (ItemInstance.ItemStaticData as SO_Consumable).ConsumableData.energyRestore.ToString();

        if(SellPrice != null)
        SellPrice.text = ItemInstance.ItemStaticData.commonData.sellPrice.ToString();
    }

    public void Use()
    {
        if(UIInventory.AudioManager != null)
        {
            UIInventory.AudioManager.PlaySFX("Choose Item");
        }
        UIMountPicker.gameObject.SetActive(true);
        UIMountPicker.SetupAmountPicker(CurrentItemInstance, EPickerAction.Use);
    }

    public void Sell()
    {
        if (UIInventory.AudioManager != null)
        {
            UIInventory.AudioManager.PlaySFX("Choose Item");
        }
        UIMountPicker.gameObject.SetActive(true);
        UIMountPicker.SetupAmountPicker(CurrentItemInstance, EPickerAction.Sell);
    }

    public void Buy()
    {
        if (UIInventory.AudioManager != null)
        {
            UIInventory.AudioManager.PlaySFX("Buy");
        }
        UIMountPicker.gameObject.SetActive(true);
        UIMountPicker.SetupAmountPicker(CurrentItemInstance, EPickerAction.Buy);
    }
}
