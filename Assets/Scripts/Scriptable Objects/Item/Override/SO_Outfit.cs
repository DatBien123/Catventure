using UnityEngine;

[System.Serializable]
public enum EOutfitType
{
    None,
    Hat,
    Shirt,
    Shoes,
    Glasses
}

[System.Serializable]
public struct OutfitData
{
    public EOutfitType Type;
}

[CreateAssetMenu(fileName = "Outfit Item Data", menuName = "Inventory System/Item Data/Outfit Item")]
public class SO_Outfit : SO_Item
{
    public OutfitData outfitData;

    public override void Use(Character character)
    {
        throw new System.NotImplementedException();
    }
}
