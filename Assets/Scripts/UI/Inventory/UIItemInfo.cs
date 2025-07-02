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
        itemIcon.sprite = item.data.icon;
        itemIcon.enabled = true;

        itemName.text = item.data.itemName;
        itemType.text = item.data.itemType.ToString();
        itemDescription.text = item.data.description;
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
