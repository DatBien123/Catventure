using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Character owner;
    public List<InventorySlot> slots = new List<InventorySlot>();

    #region Add Item
    public void AddItem(SO_Item item, int quantity)
    {
        if (!item.commonData.isStackable)
        {
            for (int i = 0; i < quantity; i++)
                slots.Add(new InventorySlot(item, 1));
            return;
        }

        int remain = quantity;

        foreach (var slot in slots)
        {
            if (slot.item.commonData.itemName == item.commonData.itemName && !slot.isFull)
            {
                int space = item.commonData.maxQuantityAllowed - slot.currentQuantity;
                int addAmount = Mathf.Min(space, remain);

                slot.Add(addAmount);
                remain -= addAmount;

                if (remain <= 0) return;
            }
        }

        while (remain > 0)
        {
            int addAmount = Mathf.Min(item.commonData.maxQuantityAllowed, remain);
            slots.Add(new InventorySlot(item, addAmount));
            remain -= addAmount;
        }
    }
    #endregion

    #region Remove Item
    public void RemoveItem(SO_Item item, int quantity)
    {
        int total = GetTotalQuantity(item);
        if (total < quantity)
        {
            Debug.LogWarning("Not enough items to remove!");
            return;
        }

        int remain = quantity;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            var slot = slots[i];
            if (slot.item.commonData.itemName == item.commonData.itemName)
            {
                if (slot.currentQuantity > remain)
                {
                    slot.Remove(remain);
                    break;
                }
                else
                {
                    remain -= slot.currentQuantity;
                    slots.RemoveAt(i);
                    if (remain <= 0) break;
                }
            }
        }
    }
    #endregion

    #region Utility
    public int GetTotalQuantity(SO_Item item)
    {
        return slots
            .Where(s => s.item.commonData.itemName == item.commonData.itemName)
            .Sum(s => s.currentQuantity);
    }

    public bool CheckItemExist(SO_Item item, int quantity)
    {
        return GetTotalQuantity(item) >= quantity;
    }

    public void ClearItem(SO_Item item)
    {
        slots.RemoveAll(s => s.item.commonData.itemName == item.commonData.itemName);
    }
    #endregion
}
