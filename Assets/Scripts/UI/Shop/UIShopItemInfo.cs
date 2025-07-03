using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIShopItemInfo : MonoBehaviour
{
    [Header("UI Elements")]
    public UnityEngine.UI.Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemDescription;
    public TextField TextField;

    public void ShowInfo(SO_Item item)
    {
        this.gameObject.SetActive(true);
        itemIcon.sprite = item.commonData.icon;
        itemIcon.enabled = true;

        itemName.text = item.commonData.itemName;
        itemType.text = item.commonData.itemType.ToString();
        itemDescription.text = item.commonData.description;
    }

    public void HideInfo()
    {
        this.gameObject.SetActive(false);
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        itemName.text = "";
        itemType.text = "";
        itemDescription.text = "";
    }
}
