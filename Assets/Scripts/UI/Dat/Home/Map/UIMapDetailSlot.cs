using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

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
    public bool IsUnlocked;
    public bool IsCompleted;

    [Header("References")]
    public UIMapDescription UIMapDescription;

    public int poolID { get; set; }
    public ObjectPooler<UIMapDetailSlot> pool { get; set; }

    private void Awake()
    {
        Start_Button.onClick.AddListener(() => StartPlay());
    }

    private void Start()
    {
        UIMapDescription = FindObjectOfType<UIMapDescription>();
    }

    public void SetupMapDetailSlot(Topic topic, bool isUnlocked, bool isCompleted)
    {
        CurrentTopic = topic;
        IsUnlocked = isUnlocked;
        IsCompleted = isCompleted;

        Image.sprite = CurrentTopic.TopicImage;
        Title.text = CurrentTopic.topicName;

        EnegyConsume.text = "Tiêu thụ: " + CurrentTopic.enegyConsumed.ToString();
        CoinReward.text = "Thưởng: " + CurrentTopic.coinReward.ToString();

        if (IsUnlocked)
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

    public void StartPlay()
    {
        // Giả sử khi hoàn thành minigame, bạn sẽ gọi một hàm để đánh dấu topic đã hoàn thành
        SceneManager.LoadScene(CurrentTopic.minigameSceneName);
        //CompleteTopic();
    }

    // Gọi hàm này khi hoàn thành minigame để cập nhật trạng thái
    public void CompleteTopic()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            var mapInstance = UIMapDescription.CurrentMapInstanceSelected;
            if (!mapInstance.CompletedTopicsIndex.Contains(CurrentTopic.index))
            {
                mapInstance.CompletedTopicsIndex.Add(CurrentTopic.index);
            }

            // Mở khóa topic tiếp theo nếu có
            int nextTopicIndex = CurrentTopic.index + 1;
            if (nextTopicIndex < mapInstance.MapData.Data.topicList.Count &&
                !mapInstance.UnlockTopicsIndex.Contains(nextTopicIndex))
            {
                mapInstance.UnlockTopicsIndex.Add(nextTopicIndex);
            }

            // Lưu trạng thái
            MapManager mapManager = FindObjectOfType<MapManager>();
            MapSaveSystem.Save(mapManager.ListMapTile.Select(tile => tile.MapInstance).ToList());

            // Cập nhật UI
            UIMapDescription.UIMapDetail.RefreshUIMap();
        }
    }
}