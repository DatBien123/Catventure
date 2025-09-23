using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class TutorialSaveData
{
    public List<TutorialPartSaveData> TutorialParts;
}

[System.Serializable]
public class TutorialPartSaveData
{
    public string TutorialName;
    public bool isPartCompleted;
}

public class TutorialSaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "tutorialData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public void SaveGame(TutorialManager tutorialManager, TutorialPart tutPartTarget)
    {
        TutorialSaveData data;

        // Kiểm tra file có tồn tại không
        if (File.Exists(SavePath))
        {
            string jsona = File.ReadAllText(SavePath);
            data = JsonUtility.FromJson<TutorialSaveData>(jsona);
            if (data == null)
            {
                data = new TutorialSaveData(); // Tạo mới nếu file hỏng
            }
        }
        else
        {
            data = new TutorialSaveData(); // Tạo mới nếu file chưa tồn tại
        }

        // Kiểm tra nếu danh sách TutorialParts chưa khởi tạo
        if (data.TutorialParts == null)
        {
            data.TutorialParts = new List<TutorialPartSaveData>();
        }

        // Tìm phần tử có cùng TutorialName
        TutorialPartSaveData existingPart = data.TutorialParts.Find(p => p.TutorialName == tutPartTarget.TutorialName);

        if (existingPart != null)
        {
            // Nếu đã tồn tại, chỉ cập nhật isPartCompleted
            existingPart.isPartCompleted = tutPartTarget.isPartCompleted;
            Debug.Log($"Updated existing part: {tutPartTarget.TutorialName}, isPartCompleted = {tutPartTarget.isPartCompleted}");
        }
        else
        {
            // Nếu chưa tồn tại, thêm mới
            data.TutorialParts.Add(new TutorialPartSaveData
            {
                TutorialName = tutPartTarget.TutorialName,
                isPartCompleted = tutPartTarget.isPartCompleted
            });
            Debug.Log($"Added new part: {tutPartTarget.TutorialName}, isPartCompleted = {tutPartTarget.isPartCompleted}");
        }

        // Ghi lại dữ liệu vào file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Tutorial data saved to: " + SavePath);
    }

    public void LoadGame(TutorialManager tutorialManager)
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Tutorial save file not found! Creating default file.");
            //SaveGame(tutorialManager, new TutorialPart()); // Tạo file mới với trạng thái mặc định
            return;
        }

        string json = File.ReadAllText(SavePath);
        TutorialSaveData data = JsonUtility.FromJson<TutorialSaveData>(json);

        if (data == null || data.TutorialParts == null)
        {
            Debug.LogWarning("Invalid tutorial save data!");
            return;
        }

        for (int i = 0; i < tutorialManager.tutorialDatabase.TutorialParts.Count; i++)
        {
            var part = tutorialManager.tutorialDatabase.TutorialParts[i]; // Lấy bản sao
            var savedPart = data.TutorialParts.Find(p => p.TutorialName == part.TutorialName);
            if (savedPart != null)
            {
                // Tạo một bản sao mới của part và gán lại giá trị
                var updatedPart = part;
                updatedPart.isPartCompleted = savedPart.isPartCompleted;
                tutorialManager.tutorialDatabase.TutorialParts[i] = updatedPart; // Gán lại vào danh sách
            }
        }

        Debug.Log("Tutorial data loaded from: " + SavePath);
    }

    public static void Save(TutorialManager tutorialManager, TutorialPart tutorialPartTarget)
    {
        FindObjectOfType<TutorialSaveSystem>().SaveGame(tutorialManager, tutorialPartTarget);
    }

    public static void Load(TutorialManager tutorialManager)
    {
        FindObjectOfType<TutorialSaveSystem>().LoadGame(tutorialManager);
    }
}