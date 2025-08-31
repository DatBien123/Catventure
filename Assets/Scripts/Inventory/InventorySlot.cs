using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemInstance ItemInstance;

    public bool isFull
    {
        get
        {
            return (ItemInstance.Quantity >= ItemInstance.ItemStaticData.commonData.maxQuantityAllowed);
        }
    }
    public InventorySlot(ItemInstance ItemInstance)
    {
        this.ItemInstance = ItemInstance;
    }

    public void Add(int amount)
    {
        ItemInstance.Quantity += amount;
    }

    public void Remove(int amount)
    {
        ItemInstance.Quantity -= amount;
        if (ItemInstance.Quantity < 0) ItemInstance.Quantity = 0;
    }
}
