using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public List<CardInstance> cardInstances;
    public int CardPerCouplePage = 8;

    private void Start()
    {
        BookSaveSystem.Load(this); // Tải trạng thái isUnlock từ JSON
    }

    #region [Data]
    public void UnlockCard(string cardID, bool isUnlock)
    {
        CardInstance cardInstance = cardInstances.Find(c => c.CardData.Data.ID == cardID);
        if (cardInstance != null)
        {
            cardInstance.isUnlock = isUnlock;
            BookSaveSystem.Save(this); // Lưu ngay khi thay đổi
        }
    }
    #endregion

    #region [Actions]
    public List<TempCardInstance> GetTempCards()
    {
        List<TempCardInstance> result = new List<TempCardInstance>();

        foreach (CardInstance card in cardInstances)
        {
            if (card.CardData.Data.Type == CardType.Temp)
            {
                result.Add(new TempCardInstance(card.CardData, card.isUnlock));
            }
        }

        return result;
    }

    public List<FoodCardInstance> GetFoodCards()
    {
        List<FoodCardInstance> result = new List<FoodCardInstance>();

        foreach (CardInstance card in cardInstances)
        {
            if (card.CardData.Data.Type == CardType.Food)
            {
                result.Add(new FoodCardInstance(card.CardData, card.isUnlock));
            }
        }

        return result;
    }

    public List<VocabularyCardInstance> GetVocabularyCards()
    {
        List<VocabularyCardInstance> result = new List<VocabularyCardInstance>();

        foreach (CardInstance card in cardInstances)
        {
            if (card.CardData.Data.Type == CardType.Volcabulary)
            {
                result.Add(new VocabularyCardInstance(card.CardData, card.isUnlock));
            }
        }

        return result;
    }
    #endregion
}