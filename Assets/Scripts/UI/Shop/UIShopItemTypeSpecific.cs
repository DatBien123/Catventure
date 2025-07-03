using TMPro;
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
        }

        isSelected = true;
        uiShop.currentShopItemSpecificType = this;
        uiShop.RefreshShopUI();
    }

    public void SetUp(EConsumableType consumable = EConsumableType.None, EOutfitType outfit = EOutfitType.None)
    {
        consumableType = consumable;
        outfitType = outfit;
        if(consumableType == EConsumableType.None)textMeshProUGUI.text = outfitType.ToString();
        else textMeshProUGUI.text = consumableType.ToString();
    }
}