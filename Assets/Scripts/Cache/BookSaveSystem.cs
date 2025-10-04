using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class BookSaveData
{
    public List<SavedCardData> Cards;
}

[System.Serializable]
public class SavedCardData
{
    public string ID; // ID từ CardData.ID
    public bool isUnlock;
}

public class BookSaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "bookData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public void SaveGame(BookManager bookManager)
    {
        BookSaveData data = new BookSaveData
        {
            Cards = new List<SavedCardData>()
        };

        // Lưu trạng thái các card
        foreach (var cardInstance in bookManager.cardInstances)
        {
            if (cardInstance.CardData != null && !string.IsNullOrEmpty(cardInstance.CardData.Data.ID))
            {
                data.Cards.Add(new SavedCardData
                {
                    ID = cardInstance.CardData.Data.ID,
                    isUnlock = cardInstance.isUnlock
                });
            }
            else
            {
                Debug.LogWarning($"Skipping card with invalid ID: {cardInstance.CardData?.Data.Name}");
            }
        }

        // Chuyển thành JSON và lưu vào file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Book data saved to: " + SavePath);
    }

    public void LoadGame(BookManager bookManager)
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Book save file not found! Initializing with default data.");
            SaveGame(bookManager); // Tạo file mặc định nếu chưa có (với isUnlock mặc định từ cardInstances)
            return;
        }

        // Đọc và parse JSON
        string json = File.ReadAllText(SavePath);
        BookSaveData data = JsonUtility.FromJson<BookSaveData>(json);

        if (data == null || data.Cards == null)
        {
            Debug.LogWarning("Invalid book save data! Initializing with default data.");
            SaveGame(bookManager);
            return;
        }

        // Đồng bộ trạng thái từ JSON vào cardInstances
        foreach (var savedCard in data.Cards)
        {
            CardInstance cardInstance = bookManager.cardInstances.Find(c => c.CardData.Data.ID == savedCard.ID);
            if (cardInstance != null)
            {
                cardInstance.isUnlock = savedCard.isUnlock;
            }
            else
            {
                Debug.LogWarning($"Card with ID {savedCard.ID} not found in BookManager!");
            }
        }

        Debug.Log("Book data loaded from: " + SavePath);
    }

    public static void Save(BookManager bookManager)
    {
        FindObjectOfType<BookSaveSystem>().SaveGame(bookManager);
    }

    public static void Load(BookManager bookManager)
    {
        FindObjectOfType<BookSaveSystem>().LoadGame(bookManager);
    }
}