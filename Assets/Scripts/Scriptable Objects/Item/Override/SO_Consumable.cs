using UnityEngine;

[System.Serializable]
public enum EConsumableType
{
    None,
    Food,
    Vegetable
}

[System.Serializable]
public struct ConsumableData
{
    public EConsumableType ConsumableType;
}

[CreateAssetMenu(fileName = "Consumable Item Data", menuName = "Inventory System/Item Data/Consumable Item")]
public class SO_Consumable : SO_Item
{
    public ConsumableData ConsumableData;

    public override void Use(Character character)
    {
        throw new System.NotImplementedException();
    }
}
