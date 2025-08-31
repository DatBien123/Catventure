using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Outfit Items")]
    public List<OutfitInstance> ShopOutfitItems = new List<OutfitInstance>();
    [Header("Consumable Items")]
    public List<ConsumableInstance> ShopConsumableItems = new List<ConsumableInstance>();

    public Inventory playerInventory;

    public int playerGold = 100000000;

    public void TryBuy(ItemInstance ItemInstance)
    {
        if (playerGold >= ItemInstance.ItemStaticData.commonData.price)
        {
            playerGold -= ItemInstance.ItemStaticData.commonData.price;
            playerInventory.AddItem(ItemInstance); // hoặc quantity tuỳ bạn
            Debug.Log("Đã mua: " + ItemInstance.ItemStaticData.commonData.itemName);
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }
}