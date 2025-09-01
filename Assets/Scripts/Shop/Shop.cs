using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public CharacterPlayer owner;
    public List<SO_Item> slots = new List<SO_Item>();

    public Inventory Inventory;

    public int playerGold = 100000000;

    public void TryBuy(ItemInstance ItemInstance)
    {
        if (playerGold >= ItemInstance.ItemStaticData.commonData.price)
        {
            playerGold -= ItemInstance.ItemStaticData.commonData.price;
            Inventory.AddItem(ItemInstance); // hoặc quantity tuỳ bạn
            Debug.Log("Đã mua: " + ItemInstance.ItemStaticData.commonData.itemName);
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }
}