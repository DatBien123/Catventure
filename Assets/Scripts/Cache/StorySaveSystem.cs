using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class StorySaveData
{
    public List<SavedStoryData> Stories;
}

[System.Serializable]
public class SavedStoryData
{
    public string ID; // ID từ StoryData.ID
    public bool isUnlock;
}

public class StorySaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "storyData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public void SaveGame(StoryManager storyManager)
    {
        StorySaveData data = new StorySaveData
        {
            Stories = new List<SavedStoryData>()
        };

        // Lưu trạng thái các story
        foreach (var storyInstance in storyManager.Stories)
        {
            if (storyInstance.StoryData != null && !string.IsNullOrEmpty(storyInstance.StoryData.Data.ID))
            {
                data.Stories.Add(new SavedStoryData
                {
                    ID = storyInstance.StoryData.Data.ID,
                    isUnlock = storyInstance.isUnlock
                });
            }
            else
            {
                Debug.LogWarning($"Skipping story with invalid ID or null StoryData: {storyInstance.StoryData?.Data.Name}");
            }
        }

        // Chuyển thành JSON và lưu vào file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Story data saved to: {SavePath} at {DateTime.Now}");
    }

    public void LoadGame(StoryManager storyManager)
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Story save file not found! Initializing with default data.");
            SaveGame(storyManager); // Tạo file mặc định nếu chưa có
            return;
        }

        // Đọc và parse JSON
        string json = File.ReadAllText(SavePath);
        StorySaveData data = JsonUtility.FromJson<StorySaveData>(json);

        if (data == null || data.Stories == null)
        {
            Debug.LogWarning("Invalid story save data! Initializing with default data.");
            SaveGame(storyManager);
            return;
        }

        // Đồng bộ trạng thái từ JSON vào Stories
        foreach (var savedStory in data.Stories)
        {
            StoryInstance storyInstance = storyManager.Stories.Find(s => s.StoryData.Data.ID == savedStory.ID);
            if (storyInstance != null)
            {
                storyInstance.isUnlock = savedStory.isUnlock;
            }
            else
            {
                Debug.LogWarning($"Story with ID {savedStory.ID} not found in StoryManager!");
            }
        }

        Debug.Log($"Story data loaded from: {SavePath} at {DateTime.Now}");
    }

    public static void Save(StoryManager storyManager)
    {
        StorySaveSystem system = FindObjectOfType<StorySaveSystem>();
        if (system != null) system.SaveGame(storyManager);
    }

    public static void Load(StoryManager storyManager)
    {
        StorySaveSystem system = FindObjectOfType<StorySaveSystem>();
        if (system != null) system.LoadGame(storyManager);
    }
}