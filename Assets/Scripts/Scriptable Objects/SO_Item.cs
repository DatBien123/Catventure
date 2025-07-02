using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Consumable,
    Equipment,
    Material,
    Quest,
    Misc
}
[System.Serializable]
public struct ItemData
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public ItemType itemType;
    public bool isStackable;
    public int maxQuantityAllowed;
    public int price;
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Inventory System/Item Data")]
public class SO_Item : ScriptableObject, IUsable
{
    public ItemData data;

    public void Use(Character character)
    {
        
    }
}
