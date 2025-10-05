using UnityEngine;
using UnityEngine.UI;

public class UIBookSlot : MonoBehaviour
{
    [Header("Book Slot Display")]
    public Image Image;
    public Button Button;

    [Header("Reference")]
    public UIBook UIBook;
    public CardInstance CurrentCard;

    [Header("Layer Lock")]
    public Image TemLayerLock;
    public Image VolcabularyLayerLock;
    public Image FoodLayerLock;

    public void Awake()
    {
        
    }
    public void Start()
    {
        Button.onClick.AddListener(() => ShowBookSlotInfo());
    }

    public void SetupBookSlot(CardInstance card)
    {
        CurrentCard = card;
        Image.sprite = card.CardData.Data.Icon;

        FoodLayerLock.gameObject.SetActive(false);
        TemLayerLock.gameObject.SetActive(false);
        VolcabularyLayerLock.gameObject.SetActive(false);

        if (card.CardData.Data.Type == CardType.Food || card.CardData.Data.Type == CardType.Vegetable)
        {
            if (card.isUnlock)
            {
                if(FoodLayerLock != null)
                {
                    FoodLayerLock.gameObject.SetActive(false);
                }
            }
            else
            {
                if (FoodLayerLock != null)
                {
                    FoodLayerLock.gameObject.SetActive(true);
                }
            }
        }
        else if(card.CardData.Data.Type == CardType.Temp)
        {
            if (card.isUnlock)
            {
                if (TemLayerLock != null)
                {
                    TemLayerLock.gameObject.SetActive(false);
                }
            }
            else
            {
                if (TemLayerLock != null)
                {
                    TemLayerLock.gameObject.SetActive(true);
                }
            }
        }
        else if(card.CardData.Data.Type == CardType.Volcabulary)
        {
            if (card.isUnlock)
            {
                if (VolcabularyLayerLock != null)
                {
                    VolcabularyLayerLock.gameObject.SetActive(false);
                }
            }
            else
            {
                if (VolcabularyLayerLock != null)
                {
                    VolcabularyLayerLock.gameObject.SetActive(true);
                }
            }
        }
    }
    public void RemoveDataBookSlot()
    {
        CurrentCard = null;
        Image.sprite = null;
        FoodLayerLock.gameObject.SetActive(false);
        TemLayerLock.gameObject.SetActive(false);
        VolcabularyLayerLock.gameObject.SetActive(false);
    }
    public void ShowBookSlotInfo()
    {
        if(CurrentCard != null && CurrentCard.isUnlock)
        {
            Debug.Log("Show Book Slot info: " + CurrentCard.CardData.Data.Name);
            UIBook.BookSlotDetail.SetupBookSlotDetail(CurrentCard);
            UIBook.BookSlotDetail.gameObject.SetActive(true);
        }
    }
}
