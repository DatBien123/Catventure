using UnityEngine;

[System.Serializable]
public enum EOutfitType
{
    None,
    Hat,
    Shirt,
    Glasses,
    HandStuff
}

[System.Serializable]
public struct OutfitData
{
    public EOutfitType Type;
    public AnimationClip equipClip;
}

[CreateAssetMenu(fileName = "Outfit Item Data", menuName = "Inventory System/Item Data/Outfit Item")]
public class SO_Outfit : SO_Item
{
    public OutfitData outfitData;

    public override void Use(Character character)
    {
        character.animator.CrossFadeInFixedTime(outfitData.equipClip.name, .2f);
    }
}
