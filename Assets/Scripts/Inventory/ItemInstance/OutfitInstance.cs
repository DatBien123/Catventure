using UnityEngine;

[System.Serializable]
public class OutfitInstance : ItemInstance
{
    public OutfitInstance(SO_Item item, int quantity, bool isEquiped) : base(item, quantity, isEquiped)
    {
        this.ItemStaticData = item;
        this.Quantity = quantity;
        this.IsEquiped = isEquiped;
    }
}
