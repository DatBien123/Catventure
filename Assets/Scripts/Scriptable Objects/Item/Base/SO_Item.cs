using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Shirt,
    Glasses,
    HandStuff,
    Hat,
    Consumable,
    Wing,
    Crops
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
    public int sellPrice;
}
[System.Serializable]
public struct ItemDisplayData
{
    public Color descriptionColor;

}

[CreateAssetMenu(fileName = "Item Data", menuName = "Inventory System/Item Data")]
public abstract class SO_Item : ScriptableObject, IUsable
{
    public ItemData commonData;
    public ItemDisplayData displayData;

    public abstract void Buy(CharacterPlayer character);
    public abstract void Sell(CharacterPlayer character);
    public abstract void Use(CharacterPlayer character);
}
