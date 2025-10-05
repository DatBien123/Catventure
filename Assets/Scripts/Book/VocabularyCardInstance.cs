using UnityEngine;

[System.Serializable]
public class VocabularyCardInstance : CardInstance
{
    public VocabularyCardInstance(SO_Card card, bool unlock) : base(card, unlock)
    {
        this.CardData = card;
        this.isUnlock = unlock;
    }
}
