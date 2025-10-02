using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMapDetailSlot : MonoBehaviour, IObjectPool<UIMapDetailSlot>
{
    [Header("Component")]
    public Image Image;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI EnegyConsume;
    public TextMeshProUGUI CoinReward;
    public GameObject LockedUI;

    [Header("Data")]
    public Button Start_Button;

    [Header("Data")]
    public Topic CurrentTopic;

    
    public int poolID { get; set; }
    public ObjectPooler<UIMapDetailSlot> pool { get; set; }

    private void Awake()
    {
        Start_Button.onClick.AddListener(() => Start());
    }
    public void SetupMapDetailSlot(Topic topic)
    {
        CurrentTopic = topic;
        Image.sprite = CurrentTopic.TopicImage;
        Title.text = CurrentTopic.topicName;

        EnegyConsume.text = "Tiêu thụ: " +  CurrentTopic.enegyConsumed.ToString();
        CoinReward.text = "Thưởng: " + CurrentTopic.coinReward.ToString();

        if (CurrentTopic.isUnlock)
        {
            LockedUI.SetActive(false);
            EnegyConsume.gameObject.SetActive(true);
            CoinReward.gameObject.SetActive(true);
            Start_Button.gameObject.SetActive(true);
        }
        else
        {
            LockedUI.SetActive(true);
            EnegyConsume.gameObject.SetActive(false);
            CoinReward.gameObject.SetActive(false);
            Start_Button.gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        
    }

}
