using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class MapSaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "mapData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    [Header("Map Data")]
    [Tooltip("Danh sách tất cả các SO_Map để tải và lưu dữ liệu.")]
    [SerializeField] private List<SO_Map> AllMaps;

    // Lưu toàn bộ danh sách MapInstance vào tệp JSON
    public void SaveMapData(List<MapInstance> mapInstances)
    {
        if (mapInstances == null || mapInstances.Count == 0)
        {
            Debug.LogWarning("MapSaveSystem: No map instances to save.");
            return;
        }

        List<SerializableMapInstance> serializableMaps = new List<SerializableMapInstance>();

        foreach (var mapInstance in mapInstances)
        {
            if (mapInstance.MapData == null || mapInstance.MapData.Data.Name == null)
            {
                Debug.LogWarning("MapSaveSystem: Invalid map instance or map name.");
                continue;
            }

            SerializableMapInstance serializableMap = new SerializableMapInstance
            {
                MapName = mapInstance.MapData.Data.Name,
                MapIndex = mapInstance.MapData.Data.index,
                IsUnlock = mapInstance.isUnlock,
                IsPlayUnlock = mapInstance.isPlayUnlock,
                IsSelected = mapInstance.isSelected,
                UnlockTopicsIndex = mapInstance.UnlockTopicsIndex ?? new List<int>(),
                CompletedTopicsIndex = mapInstance.CompletedTopicsIndex ?? new List<int>()
            };
            serializableMaps.Add(serializableMap);
        }

        SerializableMapData saveData = new SerializableMapData { Maps = serializableMaps };
        string json = JsonUtility.ToJson(saveData, true);

        try
        {
            File.WriteAllText(SavePath, json);
            Debug.Log("MapSaveSystem: Saved map data to " + SavePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("MapSaveSystem: Failed to save map data. Error: " + e.Message);
        }
    }

    // Tải dữ liệu map từ tệp JSON
    public List<MapInstance> LoadMapData()
    {
        List<MapInstance> mapInstances = new List<MapInstance>();

        if (AllMaps == null || AllMaps.Count == 0)
        {
            Debug.LogWarning("MapSaveSystem: No SO_Map provided for loading.");
            return mapInstances;
        }

        // Log tất cả các map trong AllMaps để debug
        Debug.Log("MapSaveSystem: Available SO_Maps in AllMaps:");
        foreach (var map in AllMaps)
        {
            Debug.Log($" - Name: {map.Data.Name}, Index: {map.Data.index}");
        }

        // Nếu tệp không tồn tại, khởi tạo danh sách MapInstance mặc định
        if (!File.Exists(SavePath))
        {
            foreach (var map in AllMaps)
            {
                bool isUnlock = map.Data.index == 0; // Mở khóa map với index = 0 (Hà Nội)
                mapInstances.Add(new MapInstance(
                    map,
                    isUnlock ? new List<int> { 0 } : new List<int>(), // Topic với index = 0 được mở khóa cho map index = 0
                    new List<int>(),
                    isUnlock,
                    isUnlock, // Truyền isUnlock
                    isUnlock  // Truyền isSelected, đặt giống isUnlock cho mặc định
                ));
                Debug.Log($"MapSaveSystem: Initialized map - Name: {map.Data.Name}, Index: {map.Data.index}, IsUnlock: {isUnlock}");
            }
            Debug.Log("MapSaveSystem: No save file found, initialized default map data with map index 0 unlocked.");
            return mapInstances;
        }

        // Đọc dữ liệu từ tệp JSON
        try
        {
            string json = File.ReadAllText(SavePath);
            SerializableMapData saveData = JsonUtility.FromJson<SerializableMapData>(json);

            if (saveData == null || saveData.Maps == null)
            {
                Debug.LogWarning("MapSaveSystem: Invalid save data, initializing default.");
                foreach (var map in AllMaps)
                {
                    bool isUnlock = map.Data.index == 0;
                    mapInstances.Add(new MapInstance(
                        map,
                        isUnlock ? new List<int> { 0 } : new List<int>(),
                        new List<int>(),
                        isUnlock,
                        isUnlock,
                        isUnlock
                    ));
                }
                return mapInstances;
            }

            // Khôi phục danh sách MapInstance
            foreach (var serializableMap in saveData.Maps)
            {
                SO_Map map = AllMaps.Find(m => m.Data.Name == serializableMap.MapName && m.Data.index == serializableMap.MapIndex);
                if (map != null)
                {
                    mapInstances.Add(new MapInstance(
                        map,
                        serializableMap.UnlockTopicsIndex ?? new List<int>(),
                        serializableMap.CompletedTopicsIndex ?? new List<int>(),
                        serializableMap.IsUnlock,
                        serializableMap.IsSelected,
                        serializableMap.IsPlayUnlock
                    ));
                    Debug.Log($"MapSaveSystem: Loaded map - Name: {map.Data.Name}, Index: {map.Data.index}, IsUnlock: {mapInstances.Last().isUnlock}, IsSelected: {mapInstances.Last().isSelected}");
                }
                else
                {
                    Debug.LogError($"MapSaveSystem: Failed to match map - Name: {serializableMap.MapName}, Index: {serializableMap.MapIndex} not found in AllMaps.");
                }
            }

            if (mapInstances.Count == 0)
            {
                Debug.LogWarning("MapSaveSystem: No maps loaded from file, falling back to default initialization.");
                foreach (var map in AllMaps)
                {
                    bool isUnlock = map.Data.index == 0;
                    mapInstances.Add(new MapInstance(
                        map,
                        isUnlock ? new List<int> { 0 } : new List<int>(),
                        new List<int>(),
                        isUnlock,
                        isUnlock,
                        isUnlock
                    ));
                }
            }

            Debug.Log("MapSaveSystem: Loaded map data from " + SavePath + ", Total maps: " + mapInstances.Count);
        }
        catch (System.Exception e)
        {
            Debug.LogError("MapSaveSystem: Failed to load map data. Error: " + e.Message);
            foreach (var map in AllMaps)
            {
                bool isUnlock = map.Data.index == 0;
                mapInstances.Add(new MapInstance(
                    map,
                    isUnlock ? new List<int> { 0 } : new List<int>(),
                    new List<int>(),
                    isUnlock,
                    isUnlock,
                    isUnlock
                ));
            }
        }

        return mapInstances;
    }

    // Lấy map đang được chọn (isSelected = true)
    public MapInstance GetSelectedMap()
    {
        List<MapInstance> mapInstances = LoadMapData();
        MapInstance selectedMap = mapInstances.FirstOrDefault(map => map.isSelected);
        if (selectedMap == null)
        {
            Debug.LogWarning("MapSaveSystem: No map with isSelected = true found.");
        }
        else
        {
            Debug.Log($"MapSaveSystem: Found selected map - Name: {selectedMap.MapData.Data.Name}, Index: {selectedMap.MapData.Data.index}");
        }
        return selectedMap;
    }

    // Kiểm tra xem có map nào được chọn không
    public bool HasSelectedMap()
    {
        List<MapInstance> mapInstances = LoadMapData();
        bool hasSelected = mapInstances.Any(map => map.isSelected);
        if (!hasSelected)
        {
            Debug.LogWarning("MapSaveSystem: No map is selected.");
        }
        return hasSelected;
    }

    // Xóa dữ liệu lưu trữ
    public void ClearSaveData()
    {
        if (File.Exists(SavePath))
        {
            try
            {
                File.Delete(SavePath);
                Debug.Log("MapSaveSystem: Save data cleared.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("MapSaveSystem: Failed to clear save data. Error: " + e.Message);
            }
        }
    }

    // Gọi để lưu dữ liệu map, ví dụ khi thay đổi trạng thái map
    public static void Save(List<MapInstance> mapInstances)
    {
        MapSaveSystem instance = FindObjectOfType<MapSaveSystem>();
        if (instance != null)
        {
            instance.SaveMapData(mapInstances);
        }
        else
        {
            Debug.LogError("MapSaveSystem: No instance found to save map data!");
        }
    }

    // Gọi để tải dữ liệu map, ví dụ khi bắt đầu game
    public static List<MapInstance> Load()
    {
        MapSaveSystem instance = FindObjectOfType<MapSaveSystem>();
        if (instance != null)
        {
            return instance.LoadMapData();
        }
        else
        {
            Debug.LogError("MapSaveSystem: No instance found to load map data!");
            return new List<MapInstance>();
        }
    }

    // Gọi để lấy map được chọn
    public static MapInstance GetSelected()
    {
        MapSaveSystem instance = FindObjectOfType<MapSaveSystem>();
        if (instance != null)
        {
            return instance.GetSelectedMap();
        }
        else
        {
            Debug.LogError("MapSaveSystem: No instance found to get selected map!");
            return null;
        }
    }

    // Gọi để kiểm tra có map được chọn không
    public static bool HasSelected()
    {
        MapSaveSystem instance = FindObjectOfType<MapSaveSystem>();
        if (instance != null)
        {
            return instance.HasSelectedMap();
        }
        else
        {
            Debug.LogError("MapSaveSystem: No instance found to check selected map!");
            return false;
        }
    }

    // Gọi để xóa dữ liệu lưu trữ
    public static void Clear()
    {
        MapSaveSystem instance = FindObjectOfType<MapSaveSystem>();
        if (instance != null)
        {
            instance.ClearSaveData();
        }
        else
        {
            Debug.LogError("MapSaveSystem: No instance found to clear save data!");
        }
    }

    [System.Serializable]
    private class SerializableMapInstance
    {
        public string MapName;
        public int MapIndex;
        public bool IsUnlock;
        public bool IsSelected;
        public bool IsPlayUnlock;
        public List<int> UnlockTopicsIndex;
        public List<int> CompletedTopicsIndex;
    }

    [System.Serializable]
    private class SerializableMapData
    {
        public List<SerializableMapInstance> Maps;
    }
}