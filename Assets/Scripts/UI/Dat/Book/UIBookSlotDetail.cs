using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBookSlotDetail : MonoBehaviour
{
    [Header("Book Detail UI")]
    public Image Image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI AdditiveValue;

    public Image ResourceAddtiveImage;
    public Sprite EnegyImg;

    [Header("Button")]
    public Button SpellButton;

    [Header("Reference")]
    public UIBook UIBook;
    public RectTransform ImageRectTransform;
    public CardInstance CurrentCard;
    public GameObject TempDetail;
    public GameObject BenefitUI;

    private void Awake()
    {
    }

    private void OnDisable()
    {
        if(UIBook.AudioManager != null)
        {
            UIBook.AudioManager.PlaySFX("Close");
        }
    }
    private void OnEnable()
    {
        TempDetail.transform.localScale = Vector3.zero; // Start from zero scale
        TempDetail.transform.DOScale(1.1f, 0.3f) // Zoom to 1.1f slightly overshooting
            .SetEase(Ease.OutBack) // Adds a back effect for the overshoot
            .OnComplete(() => TempDetail.transform.DOScale(1f, 0.2f)); // Settle back to 1f

        if (UIBook.AudioManager != null)
        {
            UIBook.AudioManager.PlaySFX("Click Function Yabis");
        }
    }
    void Start()
    {
        SpellButton.onClick.AddListener(() => Spell());
    }

    public void SetupBookSlotDetail(CardInstance card)
    {
        CurrentCard = card;
        if(CurrentCard.CardData.Data.Type == CardType.Food || CurrentCard.CardData.Data.Type == CardType.Vegetable)
        {
            ImageRectTransform.sizeDelta = new Vector2(217.5f, 217.5f);
            BenefitUI.SetActive(true);


            AdditiveValue.text ="+ " +( card.CardData.Data.additiveValue * 100).ToString() + "%";
            if (card.CardData.Data.Type == CardType.Food)
            {
                ResourceAddtiveImage.sprite = EnegyImg;
            }
            else if (card.CardData.Data.Type == CardType.Vegetable)
            {
                ResourceAddtiveImage.sprite = card.CardData.Data.Icon;
            }
        }
        else
        {
            ImageRectTransform.sizeDelta = new Vector2(150, 217.5f);
            BenefitUI.SetActive(false);
        }

        Image.sprite = card.CardData.Data.Icon;
        Name.text = card.CardData.Data.Name;
        Description.text = card.CardData.Data.description;
    }
    public void Spell()
    {
        if (CurrentCard != null)
        {
            //if(CurrentCard.Data.SpellClip != null) 
        }
    }
}
