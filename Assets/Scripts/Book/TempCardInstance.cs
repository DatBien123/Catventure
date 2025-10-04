using UnityEngine;

[System.Serializable]
public class TempCardInstance : CardInstance
{
    public TempCardInstance(SO_Card card, bool unlock) : base(card, unlock)
    {
        this.CardData = card;
        this.isUnlock = unlock;
    }
}
