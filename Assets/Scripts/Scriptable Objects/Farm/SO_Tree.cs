
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public enum ESeason
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
    [System.Serializable]
    public struct ClodData
    {
        public Sprite clodDryImage;
        public Sprite clodWetImage;
    }

    [System.Serializable]
    public struct TreeStageData
    {
        [Header("Mô tả hình ảnh của cây ở trạng thái này")]
        public Sprite stageImage;

        [Header("Clod Data")]
        public ClodData clodData;

        [Header("Thời gian cần để chuyển dịch sang trạng thái tiếp theo")]
        public float transitionTime;

        [Header("Có phải là trạng thái cuối cùng hay không ?")]
        [Tooltip("Phụ thuộc vào trạng thái cuối cùng để quyết định là có thu hoạch được hay không")]
        public bool isFinalStage;

        public Vector3 positionOffset;
    }
    [System.Serializable]
    public struct RewardData
    {
        [Header("Sản lượng thu được sau khi thu hoạch: ")]
        public int Harvests;
        [Header("Kinh nghiệm thu được sau khi thu hoạch: ")]
        public float ExpBonus;
        [Header("Tiền thu được sau khi bán (tính theo từng đơn vị của sản lượng): ")]
        public float SellPrice;
    }
    [System.Serializable]
    public struct TreeData
    {
        [Header("Mùa ưa thích")]
        public ESeason favoriteSeason;
        [Header("Giá trị nhận được sau khi thu hoạch")]
        public RewardData rewardData;
        [Header("Trạng thái của cây: ")]
        public List<TreeStageData> stageDatas;
        [Header("Object")]
        public GameObject TreeWorldInstance;
    }

    [CreateAssetMenu(fileName = "Tree Data", menuName = "Farm System/Data/Tree Data")]
    public class SO_Tree : SO_Item
    {
        public TreeData data;

    public override void Use(Character character)
    {
        throw new System.NotImplementedException();
    }
}