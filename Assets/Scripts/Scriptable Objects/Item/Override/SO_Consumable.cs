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
    public int energyRestore;
}

[CreateAssetMenu(fileName = "Consumable Item Data", menuName = "Inventory System/Item Data/Consumable Item")]
public class SO_Consumable : SO_Item
{
    public ConsumableData ConsumableData;

    public override void Buy(CharacterPlayer character)
    {
        throw new System.NotImplementedException();
    }

    public override void Sell(CharacterPlayer character)
    {
        throw new System.NotImplementedException();
    }

    public override void Use(CharacterPlayer character)
    {
        character.CurrentEnergy += ConsumableData.energyRestore;
        if(character.CurrentEnergy >= character.MaxEnergy) character.CurrentEnergy = character.MaxEnergy;
    }

}
