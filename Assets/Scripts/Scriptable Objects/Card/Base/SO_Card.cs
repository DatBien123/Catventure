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
    public AudioClip SpellClip;
}

public abstract class SO_Card : ScriptableObject, IUsable
{
    public CardData Data;

    public void Buy(CharacterPlayer character)
    {
        throw new System.NotImplementedException();
    }

    public void Sell(CharacterPlayer character)
    {
        throw new System.NotImplementedException();
    }

    public abstract void Use(Character character);

    public void Use(CharacterPlayer character)
    {
        throw new System.NotImplementedException();
    }
}
