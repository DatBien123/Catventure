using UnityEngine;

[System.Serializable]
public class FoodCardInstance : CardInstance
{
    public FoodCardInstance(SO_Card card, bool unlock) : base(card, unlock)
    {
        this.CardData = card;
        this.isUnlock = unlock;
    }
}
