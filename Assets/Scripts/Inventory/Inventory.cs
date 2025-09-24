using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public CharacterPlayer owner;
    public List<InventorySlot> slots = new List<InventorySlot>();

    #region Add Item
    public void AddItem(ItemInstance ItemInstance)
    {
        if (!ItemInstance.ItemStaticData.commonData.isStackable)
        {
            for (int i = 0; i < ItemInstance.Quantity; i++)
            {
                if (ItemInstance is OutfitInstance outfitInstance)
                {
                    slots.Add(new InventorySlot(outfitInstance));
                }
                else
                {
                    slots.Add(new InventorySlot(new ItemInstance(ItemInstance.ItemStaticData, 1)));
                }
            }
            return;
        }

        int remain = ItemInstance.Quantity;

        foreach (var slot in slots)
        {
            if (slot.ItemInstance.ItemStaticData.commonData.itemName == ItemInstance.ItemStaticData.commonData.itemName && !slot.isFull)
            {
                int space = ItemInstance.ItemStaticData.commonData.maxQuantityAllowed - slot.ItemInstance.Quantity;
                int addAmount = Mathf.Min(space, remain);

                slot.Add(addAmount);
                remain -= addAmount;

                if (remain <= 0) return;
            }
        }

        while (remain > 0)
        {
            int addAmount = Mathf.Min(ItemInstance.ItemStaticData.commonData.maxQuantityAllowed, remain);

            if (ItemInstance is OutfitInstance outfitInstance)
            {
                slots.Add(new InventorySlot(new OutfitInstance(outfitInstance.ItemStaticData, addAmount, false)));
            }
            else if (ItemInstance is ConsumableInstance consumableInstance)
            {
                slots.Add(new InventorySlot(new ConsumableInstance(consumableInstance.ItemStaticData, addAmount)));
            }
            else if(ItemInstance is CropsInstance cropsInstance)
            {
                slots.Add(new InventorySlot(new CropsInstance(cropsInstance.ItemStaticData, addAmount)));
            }
            else
            {
                slots.Add(new InventorySlot(new ItemInstance(ItemInstance.ItemStaticData, addAmount)));
            }

            remain -= addAmount;
        }

        SaveSystem.Save(owner, this);
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
            if (slot.ItemInstance.ItemStaticData.commonData.itemName == item.commonData.itemName)
            {
                if (slot.ItemInstance.Quantity > remain)
                {
                    slot.Remove(remain);
                    break;
                }
                else
                {
                    remain -= slot.ItemInstance.Quantity;
                    slots.RemoveAt(i);
                    if (remain <= 0) break;
                }
            }
        }

        SaveSystem.Save(owner, this);
    }
    #endregion

    #region Utility
    public int GetTotalQuantity(SO_Item item)
    {
        return slots
            .Where(s => s.ItemInstance.ItemStaticData.commonData.itemName == item.commonData.itemName)
            .Sum(s => s.ItemInstance.Quantity);
    }

    public bool CheckItemExist(SO_Item item)
    {
        foreach(var slot in slots)
        {
            if(slot.ItemInstance.ItemStaticData.name == item.commonData.itemName) return true;

        }
        return false;
    }

    public void ClearItem(SO_Item item)
    {
        slots.RemoveAll(s => s.ItemInstance.ItemStaticData.commonData.itemName == item.commonData.itemName);
    }
    #endregion
}
