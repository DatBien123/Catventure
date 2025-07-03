using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Inventory System/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>();

    public ItemData GetItemByID(string id)
    {
        return items.Find(item => item.itemID == id);
    }
}
