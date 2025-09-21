using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using FarmSystem;
using Crops = FarmSystem.Crops;

[System.Serializable]
public class FarmData
{
    public List<SoilData> Soils;
}

[System.Serializable]
public class SoilData
{
    public string SoilName; // Tên GameObject của Soil
    public ESoilState SoilState; // Trạng thái đất
    public string TreeItemID; // itemID của SO_Tree nếu có cây
    public int CurrentStageIndex; // Giai đoạn hiện tại của cây
    public long PlantTimeTicks; // Thời gian cây được trồng (ticks)
    public RewardedData Reward; // Lưu phần thưởng khi cây ở trạng thái cuối
}

[System.Serializable]
public class RewardedData
{
    public int Harvests; // Sản lượng
    public float ExpBonus; // Kinh nghiệm
    public float SellPrice; // Giá bán mỗi đơn vị
}

public class FarmSaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "farmData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    [SerializeField] private List<SO_Item> allItemsDatabase; // Bao gồm cả SO_Tree

    public void SaveGame(FarmManager farmManager)
    {
        FarmData data = new FarmData
        {
            Soils = new List<SoilData>()
        };

        // Lưu trạng thái các ô đất
        foreach (var soil in farmManager.Soils)
        {
            SoilData soilData = new SoilData
            {
                SoilName = soil.gameObject.name,
                SoilState = soil.SoilState,
                TreeItemID = soil.CurrentTree != null ? soil.CurrentTree.TreeDataOrigin.commonData.itemID : null,
                CurrentStageIndex = soil.CurrentTree != null ? soil.CurrentTree.CurrentStageIndex : 0,
                PlantTimeTicks = soil.CurrentTree != null ? DateTime.Now.Ticks : 0,
                Reward = soil.CurrentTree != null && soil.CurrentTree.TreeCurrentStage.isFinalStage
                    ? new RewardedData
                    {
                        Harvests = soil.CurrentTree.TreeDataOrigin.data.rewardData.Harvests,
                        ExpBonus = soil.CurrentTree.TreeDataOrigin.data.rewardData.ExpBonus,
                        SellPrice = soil.CurrentTree.TreeDataOrigin.data.rewardData.SellPrice
                    }
                    : null
            };
            data.Soils.Add(soilData);
        }

        // Chuyển thành JSON và lưu vào file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Farm data saved to: " + SavePath);
    }

    public void LoadGame(FarmManager farmManager)
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Farm save file not found!");
            return;
        }

        // Đọc và parse JSON
        string json = File.ReadAllText(SavePath);
        FarmData data = JsonUtility.FromJson<FarmData>(json);

        // Load trạng thái các ô đất
        foreach (var soilData in data.Soils)
        {
            Soil soil = farmManager.Soils.Find(s => s.gameObject.name == soilData.SoilName);
            if (soil == null)
            {
                Debug.LogWarning($"Soil with name {soilData.SoilName} not found!");
                continue;
            }

            // Khôi phục trạng thái đất
            soil.SoilState = soilData.SoilState;
            soil.spriteRenderer.sprite = GetSoilSprite(soil, soilData.SoilState);

            // Khôi phục cây nếu có
            if (!string.IsNullOrEmpty(soilData.TreeItemID))
            {
                SO_Tree treeData = allItemsDatabase.Find(i => i.commonData.itemID == soilData.TreeItemID) as SO_Tree;
                if (treeData == null)
                {
                    Debug.LogWarning($"Tree with ID {soilData.TreeItemID} not found in database!");
                    continue;
                }

                // Tạo cây
                soil.CurrentTree = Instantiate(treeData.data.TreeWorldInstance, soil.transform).GetComponent<Crops>();
                soil.CurrentTree.transform.localPosition = treeData.data.stageDatas[soilData.CurrentStageIndex].positionOffset;
                soil.CurrentTree.TreeDataOrigin = treeData;
                soil.CurrentTree.TreeDataTemporary = Instantiate(treeData);

                // Tính toán giai đoạn hiện tại của cây
                long currentTicks = DateTime.Now.Ticks;
                long plantTimeTicks = soilData.PlantTimeTicks;
                float elapsedSeconds = (float)new TimeSpan(currentTicks - plantTimeTicks).TotalSeconds;

                int newStageIndex = CalculateTreeStage(treeData, soilData.CurrentStageIndex, elapsedSeconds);
                soil.CurrentTree.CurrentStageIndex = newStageIndex;
                soil.CurrentTree.TreeCurrentStage = treeData.data.stageDatas[newStageIndex];
                soil.CurrentTree.spriteRenderer.sprite = soil.CurrentTree.TreeCurrentStage.stageImage;

                // Cập nhật clod sprite
                if (soil.CurrentTree.TreeCurrentStage.clodData.clodWetImage && soil.CurrentTree.TreeCurrentStage.clodData.clodDryImage)
                {
                    soil.clodSpriteRenderer.sprite = soil.HasState(ESoilState.Dry)
                        ? soil.CurrentTree.TreeCurrentStage.clodData.clodDryImage
                        : soil.CurrentTree.TreeCurrentStage.clodData.clodWetImage;
                }
                else
                {
                    soil.clodSpriteRenderer.sprite = null;
                }

                // Nếu cây ở trạng thái cuối, đảm bảo đất có trạng thái CanHarvest
                if (soil.CurrentTree.TreeCurrentStage.isFinalStage && !soil.HasState(ESoilState.CanHarvest))
                {
                    soil.AddState(ESoilState.CanHarvest);
                }

                // Khởi động lại coroutine Growing nếu chưa ở giai đoạn cuối
                if (!soil.CurrentTree.TreeCurrentStage.isFinalStage)
                {
                    soil.CurrentTree.StartGrowing();
                }
            }
            else
            {
                soil.CurrentTree = null;
                soil.clodSpriteRenderer.sprite = null;
            }
        }

        // Cập nhật UI của FarmManager
        if (farmManager.CurrentSoilSelected != null)
        {
            farmManager.CurrentSoilSelected.OnPointerClick(null); // Gọi để cập nhật UI toolbar
        }

        Debug.Log("Farm data loaded from: " + SavePath);
    }

    private Sprite GetSoilSprite(Soil soil, ESoilState state)
    {
        if (state.HasFlag(ESoilState.HasPlant))
            return soil.HasPlantSprite;
        if (state.HasFlag(ESoilState.HavestedOrDigged))
            return soil.HavestedOrRemovePlantSprite;
        if (state.HasFlag(ESoilState.Dry))
            return soil.DrySprite;
        if (state.HasFlag(ESoilState.Wet))
            return soil.WetSprite;
        return soil.DrySprite; // Mặc định
    }

    private int CalculateTreeStage(SO_Tree treeData, int savedStageIndex, float elapsedSeconds)
    {
        float totalTime = 0f;
        int currentStage = savedStageIndex;

        for (int i = savedStageIndex; i < treeData.data.stageDatas.Count; i++)
        {
            totalTime += treeData.data.stageDatas[i].transitionTime;
            if (elapsedSeconds >= totalTime && !treeData.data.stageDatas[i].isFinalStage)
            {
                currentStage = i + 1;
            }
            else
            {
                break;
            }
        }

        return Mathf.Min(currentStage, treeData.data.stageDatas.Count - 1);
    }

    public static void Save(FarmManager farmManager)
    {
        FindObjectOfType<FarmSaveSystem>().SaveGame(farmManager);
    }

    public static void Load(FarmManager farmManager)
    {
        FindObjectOfType<FarmSaveSystem>().LoadGame(farmManager);
    }
}