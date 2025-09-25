using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public CharacterPlayer owner;
    public List<SO_Item> slots = new List<SO_Item>();

    public Inventory Inventory;

    public void TryBuy(ItemInstance itemInstance)
    {
        owner.Coin -= itemInstance.ItemStaticData.commonData.price;

        Inventory.AddItem(new ItemInstance(itemInstance.ItemStaticData, 1, false));

        SaveSystem.Save(owner, Inventory);

    }
}