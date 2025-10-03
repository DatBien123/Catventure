using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIMapDetail : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI Title;
    public Slider ProcessSlider;
    public TextMeshProUGUI Process;

    public UIMapDetailSlot mapDetailSlotPrefab;
    public Transform slotParent;

    public List<UIMapDetailSlot> uiMapDetailSlots = new List<UIMapDetailSlot>();

    [Header("References")]
    public UIMapDescription UIMapDescription;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 10;
    protected ObjectPooler<UIMapDetailSlot> pooler { get; private set; }

    #endregion

    private void Awake()
    {
        pooler = new ObjectPooler<UIMapDetailSlot>();
        pooler.Initialize(this, poolCount, mapDetailSlotPrefab, slotParent);
    }

    public void RefreshUIMap()
    {
        // 1. Giải phóng các slot cũ về pool
        foreach (var slot in uiMapDetailSlots)
        {
            pooler.Free(slot);
        }
        uiMapDetailSlots.Clear();

        // 2. Tạo slot mới
        var mapInstance = UIMapDescription.CurrentMapInstanceSelected;
        for (int i = 0; i < mapInstance.MapData.Data.topicList.Count; i++)
        {
            var topic = mapInstance.MapData.Data.topicList[i];
            UIMapDetailSlot uiSlot = pooler.GetNew();
            uiSlot.SetupMapDetailSlot(topic, mapInstance.UnlockTopicsIndex.Contains(i), mapInstance.CompletedTopicsIndex.Contains(i));
            uiMapDetailSlots.Add(uiSlot);
        }

        // 3. Sắp xếp lại danh sách UI slot theo index của topic
        uiMapDetailSlots.Sort((a, b) => a.CurrentTopic.index.CompareTo(b.CurrentTopic.index));

        // 4. Cập nhật lại thứ tự trong Hierarchy
        for (int i = 0; i < uiMapDetailSlots.Count; i++)
        {
            uiMapDetailSlots[i].transform.SetSiblingIndex(i);
        }
    }

    public void SetupMapDetail()
    {
        Title.text = "Khám phá " + UIMapDescription.CurrentMapInstanceSelected.MapData.Data.Name;
        ProcessSlider.value = GetProcess();
        Process.text = "Tiến trình khám phá: " + GetProcess().ToString("F0") + "%";

        RefreshUIMap();
    }

    public float GetProcess()
    {
        if (UIMapDescription.CurrentMapInstanceSelected == null) return 0;

        int completedTopic = UIMapDescription.CurrentMapInstanceSelected.CompletedTopicsIndex.Count;
        int totalTopics = UIMapDescription.CurrentMapInstanceSelected.MapData.Data.topicList.Count;

        return totalTopics > 0 ? (completedTopic * 100f / totalTopics) : 0;
    }
}