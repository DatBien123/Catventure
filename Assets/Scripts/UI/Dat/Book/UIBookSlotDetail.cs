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

    [Header("Button")]
    public Button SpellButton;

    [Header("Reference")]
    public UIBook UIBook;
    public RectTransform ImageRectTransform;
    public SO_Card CurrentCard;
    public GameObject TempDetail;

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
