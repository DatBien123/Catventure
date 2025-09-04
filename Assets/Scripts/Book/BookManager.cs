using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public List<SO_Card> cards;
    public int CardPerCouplePage = 8;

    public List<SO_TempCard> GetTempCards()
    {
        List<SO_TempCard> result = new List<SO_TempCard>();

        foreach (SO_Card card in cards)
        {
            if(card.Data.Type == CardType.Temp)
            {
                result.Add(card as SO_TempCard);
            }
        }

        return result;
    }

    public List<SO_FoodCard> GetFoodCards()
    {
        List<SO_FoodCard> result = new List<SO_FoodCard>();

        foreach (SO_Card card in cards)
        {
            if (card.Data.Type == CardType.Food)
            {
                result.Add(card as SO_FoodCard);
            }
        }

        return result;
    }

    public List<SO_Vocabulary> GetVocabularyCards()
    {
        List<SO_Vocabulary> result = new List<SO_Vocabulary>();

        foreach (SO_Card card in cards)
        {
            if (card.Data.Type == CardType.Volcabulary)
            {
                result.Add(card as SO_Vocabulary);
            }
        }

        return result;
    }
}
