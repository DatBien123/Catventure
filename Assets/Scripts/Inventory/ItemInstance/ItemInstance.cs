using UnityEngine;

[System.Serializable]
public class ItemInstance 
{
    public SO_Item ItemStaticData;
    public bool IsEquiped;
    public int Quantity;

    public ItemInstance(SO_Item item, int quantity, bool isEquiped = false)
    {
        ItemStaticData = item;
        this.Quantity = quantity;
        this.IsEquiped = isEquiped;
    }
}
