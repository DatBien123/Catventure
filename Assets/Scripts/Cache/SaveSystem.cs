using FarmSystem;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public bool isFirstTimeLogin;
    public int Coin;
    public int CurrentEnergy;
    public int MaxEnergy;
    public List<ItemInstanceData> InventoryItems;

    public OutfitInstanceData Hat;
    public OutfitInstanceData Shirt;
    public OutfitInstanceData Glasses;
    public OutfitInstanceData HandStuff;
    public OutfitInstanceData Wing;
}

[System.Serializable]
public class ItemInstanceData
{
    public string ItemID;
    public int Quantity;
    public bool IsEquiped;
    public string ItemType; // Để xác định loại item (OutfitInstance, ConsumableInstance, etc.)
}

[System.Serializable]
public class OutfitInstanceData
{
    public string ItemID;
    public int Quantity;
    public bool IsEquiped;
}

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "playerData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    // Tham chiếu đến danh sách ScriptableObject để ánh xạ itemID
    [SerializeField] private List<SO_Item> allItemsDatabase; // Gán trong Inspector hoặc load từ Resources

    public void SaveGame(CharacterPlayer player, Inventory inventory)
    {
        PlayerData data = new PlayerData
        {
            isFirstTimeLogin = player.isFirstTimeLogin,
            Coin = player.Coin,
            CurrentEnergy = player.CurrentEnergy,
            MaxEnergy = player.MaxEnergy,
            InventoryItems = new List<ItemInstanceData>(),
            Hat = ConvertOutfitToData(player.Hat),
            Shirt = ConvertOutfitToData(player.Shirt),
            Glasses = ConvertOutfitToData(player.Glasses),
            HandStuff = ConvertOutfitToData(player.HandStuff),
            Wing = ConvertOutfitToData(player.Wing)
        };

        // Lưu inventory
        foreach (var slot in inventory.slots)
        {
            ItemInstanceData itemData = new ItemInstanceData
            {
                ItemID = slot.ItemInstance.ItemStaticData.commonData.itemID,
                Quantity = slot.ItemInstance.Quantity,
                IsEquiped = slot.ItemInstance.IsEquiped,
                ItemType = slot.ItemInstance.GetType().Name // Lưu loại item (OutfitInstance, ConsumableInstance, etc.)
            };
            data.InventoryItems.Add(itemData);
        }

        // Chuyển thành JSON và lưu vào file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game saved to: " + SavePath);
    }

    public void LoadGame(CharacterPlayer player, Inventory inventory)
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        // Đọc và parse JSON
        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        // Load dữ liệu nhân vật
        player.isFirstTimeLogin = data.isFirstTimeLogin;
        player.Coin = data.Coin;
        player.CurrentEnergy = data.CurrentEnergy;
        player.MaxEnergy = data.MaxEnergy;

        // Xóa inventory hiện tại
        inventory.slots.Clear();

        // Load inventory
        foreach (var itemData in data.InventoryItems)
        {
            SO_Item item = allItemsDatabase.Find(i => i.commonData.itemID == itemData.ItemID);
            if (item == null)
            {
                Debug.LogWarning($"Item with ID {itemData.ItemID} not found in database!");
                continue;
            }

            ItemInstance instance;
            if (itemData.ItemType == nameof(OutfitInstance))
            {
                instance = new OutfitInstance(item, itemData.Quantity, itemData.IsEquiped);
            }
            else if (itemData.ItemType == nameof(ConsumableInstance))
            {
                instance = new ConsumableInstance(item, itemData.Quantity);
            }
            else if (itemData.ItemType == nameof(CropsInstance))
            {
                instance = new CropsInstance(item, itemData.Quantity);
            }
            else
            {
                instance = new ItemInstance(item, itemData.Quantity, itemData.IsEquiped);
            }

            inventory.AddItemLoad(instance);
        }

        // Load các outfit đang equip
        LoadOutfit(player, data.Hat, ItemType.Hat);
        LoadOutfit(player, data.Shirt, ItemType.Shirt);
        LoadOutfit(player, data.Glasses, ItemType.Glasses);
        LoadOutfit(player, data.HandStuff, ItemType.HandStuff);
        LoadOutfit(player, data.Wing, ItemType.Wing);

        // Cập nhật UI
        UIInventory uiInventory = FindObjectOfType<UIInventory>();
        if (uiInventory != null)
        {
            uiInventory.RefreshUI();
        }

        UIShop uiShop = FindObjectOfType<UIShop>();
        if (uiShop != null)
        {
            uiShop.RefreshShopUI();
        }

        Debug.Log("Game loaded from: " + SavePath);
    }

    private OutfitInstanceData ConvertOutfitToData(OutfitInstance outfit)
    {
        if (outfit == null || outfit.ItemStaticData == null || string.IsNullOrEmpty(outfit.ItemStaticData.commonData.itemID))
        {
            return null;
        }

        return new OutfitInstanceData
        {
            ItemID = outfit.ItemStaticData.commonData.itemID,
            Quantity = outfit.Quantity,
            IsEquiped = outfit.IsEquiped
        };
    }

    private void LoadOutfit(CharacterPlayer player, OutfitInstanceData data, ItemType itemType)
    {
        if (data == null || string.IsNullOrEmpty(data.ItemID)) return;

        SO_Item item = allItemsDatabase.Find(i => i.commonData.itemID == data.ItemID);
        if (item == null)
        {
            Debug.LogWarning($"Outfit with ID {data.ItemID} not found in database!");
            return;
        }

        OutfitInstance outfit = new OutfitInstance(item, data.Quantity, data.IsEquiped);
        switch (itemType)
        {
            case ItemType.Hat:
                player.Hat = outfit;
                break;
            case ItemType.Shirt:
                player.Shirt = outfit;
                break;
            case ItemType.Glasses:
                player.Glasses = outfit;
                break;
            case ItemType.HandStuff:
                player.HandStuff = outfit;
                break;
            case ItemType.Wing:
                player.Wing = outfit;
                break;
        }

        if (data.IsEquiped)
        {
            player.Wear(itemType, item.commonData.itemName);
        }
    }

    // Gọi để lưu game, ví dụ khi người chơi thoát hoặc đạt checkpoint
    public static void Save(CharacterPlayer player, Inventory inventory)
    {
        FindObjectOfType<SaveSystem>().SaveGame(player, inventory);
    }

    // Gọi để tải game, ví dụ khi bắt đầu game
    public static void Load(CharacterPlayer player, Inventory inventory)
    {
        FindObjectOfType<SaveSystem>().LoadGame(player, inventory);
    }
}