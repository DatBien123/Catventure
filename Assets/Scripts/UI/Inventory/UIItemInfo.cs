using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInfo : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemDescription;

    public void ShowInfo(SO_Item item)
    {
        itemIcon.sprite = item.commonData.icon;
        itemIcon.enabled = true;

        itemName.text = item.commonData.itemName;
        itemType.text = item.commonData.itemType.ToString();
        itemDescription.text = item.commonData.description;
    }

    public void ClearInfo()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        itemName.text = "";
        itemType.text = "";
        itemDescription.text = "";
    }
}
