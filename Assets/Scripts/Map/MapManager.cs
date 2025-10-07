using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<UIMapTile> ListMapTile;
    public List<SO_Map> AllMaps;

    private void Awake()
    {
        List<MapInstance> loadedMaps = MapSaveSystem.Load();

        // Kiểm tra và đặt isUnlock = true cho map với index = 1 nếu chưa được mở khóa
        foreach (var mapInstance in loadedMaps)
        {
            if (mapInstance.MapData.Data.index == 0 && !mapInstance.isUnlock)
            {
                mapInstance.isUnlock = true;
                // Thêm topic index = 1 vào UnlockTopicsIndex nếu chưa có
                if (!mapInstance.UnlockTopicsIndex.Contains(0))
                {
                    mapInstance.UnlockTopicsIndex.Add(0);
                }
            }
        }

        // Gán vào ListMapTile
        for (int i = 0; i < ListMapTile.Count && i < loadedMaps.Count; i++)
        {
            ListMapTile[i].MapInstance = loadedMaps[i];
            Debug.Log("Unlock: " + ListMapTile[i].MapInstance.isPlayUnlock);

            ListMapTile[i].SetupMapTile(ListMapTile[i].MapInstance.isUnlock);
        }

        // Lưu trạng thái đã cập nhật vào tệp
        MapSaveSystem.Save(ListMapTile.Select(tile => tile.MapInstance).ToList());
    }
}