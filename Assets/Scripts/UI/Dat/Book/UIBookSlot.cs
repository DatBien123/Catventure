using UnityEngine;
using UnityEngine.UI;

public class UIBookSlot : MonoBehaviour
{
    [Header("Book Slot Display")]
    public Image Image;
    public Button Button;

    [Header("Reference")]
    public UIBook UIBook;
    public SO_Card CurrentCardData;

    public void Awake()
    {
        
    }
    public void Start()
    {
        Button.onClick.AddListener(() => ShowBookSlotInfo());
    }

    public void SetupBookSlot(SO_Card card)
    {
        CurrentCardData = card;
        Image.sprite = card.Data.Icon;
    }
    public void RemoveDataBookSlot()
    {
        CurrentCardData = null;
        Image.sprite = null;
    }
    public void ShowBookSlotInfo()
    {
        if(CurrentCardData != null)
        {
            Debug.Log("Show Book Slot info: " + CurrentCardData.Data.Name);
            UIBook.BookSlotDetail.SetupBookSlotDetail(CurrentCardData);
            UIBook.BookSlotDetail.gameObject.SetActive(true);
        }
    }
}
