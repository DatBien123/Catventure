using UnityEngine;

[System.Serializable]
public class ShopSlot
{
    public ItemInstance ItemInstance;

    public ShopSlot(ItemInstance ItemInstance)
    {
        this.ItemInstance = ItemInstance;
    }
}
