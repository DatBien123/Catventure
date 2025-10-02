using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //protected GameObject poolParent;

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

        // 2. Tạo slot mới (thứ tự có thể random do pooler)
        foreach (var topic in UIMapDescription.CurrentMapSelected.Data.topicList)
        {
            UIMapDetailSlot uiSlot = pooler.GetNew();
            uiSlot.SetupMapDetailSlot(topic);
            uiMapDetailSlots.Add(uiSlot);
        }

        // 3. ✅ Sắp xếp lại danh sách UI slot theo index của topic
        uiMapDetailSlots.Sort((a, b) => a.CurrentTopic.index.CompareTo(b.CurrentTopic.index));

        // 4. ✅ Cập nhật lại thứ tự trong Hierarchy theo index
        for (int i = 0; i < uiMapDetailSlots.Count; i++)
        {
            uiMapDetailSlots[i].transform.SetSiblingIndex(i);
        }
    }

    public void SetupMapDetail()
    {
        Title.text = "Khám phá " + UIMapDescription.CurrentMapSelected.Data.Name;
        ProcessSlider.value = GetProcess();
        Process.text = "Tiến trình khám phá: " +  GetProcess().ToString() + "%";

        RefreshUIMap();
    }
    public float GetProcess()
    {
        if (UIMapDescription.CurrentMapSelected == null) return 0;

        int completedTopic = 0;
        foreach(var topic in UIMapDescription.CurrentMapSelected.Data.topicList)
        {
            if(topic.isCompleted) completedTopic++;
        }

        return completedTopic * 100 / UIMapDescription.CurrentMapSelected.Data.topicList.Count;

    }

}
