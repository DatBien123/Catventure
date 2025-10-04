using UnityEngine;

[System.Serializable]
public class CardInstance
{
    public SO_Card CardData;
    public bool isUnlock;

    public CardInstance(SO_Card card, bool unlock)
    {
        CardData = card;
        this.isUnlock = unlock;
    }
}
