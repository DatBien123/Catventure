using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<SO_Item> shopItems = new List<SO_Item>();

    public Inventory playerInventory;

    public int playerGold = 100000000;



    public void TryBuy(SO_Item item)
    {
        if (playerGold >= item.commonData.price)
        {
            playerGold -= item.commonData.price;
            playerInventory.AddItem(item, 1); // hoặc quantity tuỳ bạn
            Debug.Log("Đã mua: " + item.commonData.itemName);
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }
}