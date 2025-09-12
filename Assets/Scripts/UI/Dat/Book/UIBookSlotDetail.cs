using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBookSlotDetail : MonoBehaviour
{
    [Header("Book Detail UI")]
    public Image Image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;

    [Header("Button")]
    public Button SpellButton;

    [Header("Reference")]
    public RectTransform ImageRectTransform;
    public SO_Card CurrentCard;
    public AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = 
    }
    void Start()
    {
        SpellButton.onClick.AddListener(() => Spell());
    }

    public void SetupBookSlotDetail(SO_Card card)
    {
        CurrentCard = card;
        if(CurrentCard.Data.Type == CardType.Food)
        {
            ImageRectTransform.sizeDelta = new Vector2(217.5f, 217.5f);
        }
        else
        {
            ImageRectTransform.sizeDelta = new Vector2(150, 217.5f);
        }

        Image.sprite = card.Data.Icon;
        Name.text = card.Data.Name;
        Description.text = card.Data.description;
    }
    public void Spell()
    {
        if (CurrentCard != null)
        {
            //if(CurrentCard.Data.SpellClip != null) 
        }
    }
}
