using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public SO_Item item;

    public int currentQuantity;

    public bool isFull
    {
        get
        {
            return (currentQuantity >= item.commonData.maxQuantityAllowed); // Tinh den truong hop xau nhat la lon hon
        }
    }
    public InventorySlot(SO_Item item, int quantity)
    {
        this.item = item;
        this.currentQuantity = quantity;
    }

    public void Add(int amount)
    {
        currentQuantity += amount;
    }

    public void Remove(int amount)
    {
        currentQuantity -= amount;
        if (currentQuantity < 0) currentQuantity = 0;
    }
}
