using UnityEngine;

[System.Serializable]
public class ConsumableInstance : ItemInstance
{
    public ConsumableInstance(SO_Item item, int quantity) : base(item, quantity)
    {
        this.ItemStaticData = item;
        this.Quantity = quantity;
    }
}
