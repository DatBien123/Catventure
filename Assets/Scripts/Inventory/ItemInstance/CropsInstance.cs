using UnityEngine;

[System.Serializable]
public class CropsInstance : ItemInstance
{
    public CropsInstance(SO_Item item, int quantity) : base(item, quantity)
    {
        this.ItemStaticData = item;
        this.Quantity = quantity;
    }
}
