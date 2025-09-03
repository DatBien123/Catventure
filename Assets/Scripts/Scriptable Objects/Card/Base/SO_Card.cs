using UnityEngine;

[System.Serializable]
public enum CardType
{
    Food,
    Temp,
    Volcabulary
}
[System.Serializable]
public struct CardData
{
    public string ID;
    public string Name;
    public Sprite Icon;
    [TextArea] public string description;
    public CardType Type;
    public bool IsUnlocked;
}

//[CreateAssetMenu(fileName = "Card Data", menuName = "Collection System/Card Data")]
public abstract class SO_Card : ScriptableObject, IUsable
{
    public CardData Data;
    public abstract void Use(Character character);
}
