using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItemTypeSpecific : MonoBehaviour, IPointerDownHandler
{
    public EConsumableType consumableType;
    public EOutfitType outfitType;
    public Image backgroundImage;
    public Image iconImage;
    public TextMeshProUGUI textMeshProUGUI;

    public bool isSelected;

    [Header("References")]
    public UIShop uiShop;

    private void Awake()
    {
        uiShop = GameObject.FindAnyObjectByType<UIShop>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var shopItemTypeSpecific in uiShop.shopItemTypeSpecifics)
        {
            shopItemTypeSpecific.isSelected = false;
            shopItemTypeSpecific.OnSelect(false);
        }

        isSelected = true;
        OnSelect(true);
        uiShop.currentShopItemSpecificType = this;
        uiShop.RefreshByShopItemTypeSpecific();
    }

    public void SetUp(EConsumableType consumable = EConsumableType.None, EOutfitType outfit = EOutfitType.None)
    {
        consumableType = consumable;
        outfitType = outfit;
        if (consumableType == EConsumableType.None)
        {
            SO_ButtonData buttonData = Resources.Load<SO_ButtonData>($"Graphics/ItemSpecificType/Outfit/{outfitType}");
            iconImage.sprite = buttonData.buttonData.mainIcon;
            textMeshProUGUI.text = outfitType.ToString();
        }
        else
        {
            SO_ButtonData buttonData = Resources.Load<SO_ButtonData>($"Graphics/ItemSpecificType/Consumable/{consumable}");
            iconImage.sprite = buttonData.buttonData.mainIcon;
            textMeshProUGUI.text = consumableType.ToString();
        }
    }

    public void OnSelect(bool isSelect)
    {
        if (consumableType == EConsumableType.None)
        {
            SO_ButtonData buttonData = Resources.Load<SO_ButtonData>($"Graphics/ItemSpecificType/Outfit/{outfitType}");

            textMeshProUGUI.text = outfitType.ToString();

            if (isSelect)
            {
                backgroundImage.sprite = buttonData.buttonData.selectFieldData.backgroundSprite;
                backgroundImage.color = buttonData.buttonData.selectFieldData.backgroundColor;
                textMeshProUGUI.color = buttonData.buttonData.selectFieldData.textColor;
            }
            else
            {
                backgroundImage.sprite = buttonData.buttonData.deselectdFieldData.backgroundSprite;
                backgroundImage.color = buttonData.buttonData.deselectdFieldData.backgroundColor;
                textMeshProUGUI.color = buttonData.buttonData.deselectdFieldData.textColor;
            }

        }
        else 
        {
            SO_ButtonData buttonData = Resources.Load<SO_ButtonData>($"Graphics/ItemSpecificType/Consumable/{consumableType}");

            textMeshProUGUI.text = consumableType.ToString();

            if (isSelect)
            {
                backgroundImage.sprite = buttonData.buttonData.selectFieldData.backgroundSprite;
                backgroundImage.color = buttonData.buttonData.selectFieldData.backgroundColor;
                textMeshProUGUI.color = buttonData.buttonData.selectFieldData.textColor;
            }
            else
            {
                backgroundImage.sprite = buttonData.buttonData.deselectdFieldData.backgroundSprite;
                backgroundImage.color = buttonData.buttonData.deselectdFieldData.backgroundColor;
                textMeshProUGUI.color = buttonData.buttonData.deselectdFieldData.textColor;
            }
        }


    }
}