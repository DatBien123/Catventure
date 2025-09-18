using System.Collections.Generic;
using UnityEngine;

public class CropToolbar : MonoBehaviour
{
    public CropToolbarSlot CropSlotToolbarPrefab;
    public Transform CropToolbarTransformParent;
    public List<CropToolbarSlot> uiCropSlots = new List<CropToolbarSlot>();

    [Header("References")]
    public FarmManager FarmManager;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 10;
    protected ObjectPooler<CropToolbarSlot> pooler { get; private set; }
    #endregion

    private void Awake()
    {
        pooler = new ObjectPooler<CropToolbarSlot>();
        pooler.Initialize(this, poolCount, CropSlotToolbarPrefab, CropToolbarTransformParent);
    }

    private void Start()
    {
        RefreshCropsToolbar();
    }
    public void RefreshCropsToolbar()
    {
        foreach (var slot in uiCropSlots)
        {
            pooler.Free(slot);
        }
        uiCropSlots.Clear();

        //Lấy ra toàn bộ các loại hạt rau củ có trong inventory

        List<CropsInstance> cropsItemInstances = new List<CropsInstance>();

        foreach(InventorySlot invSlot in FarmManager.CharacterPlayer.Inventory.slots)
        {
            if(invSlot.ItemInstance.ItemStaticData is SO_Tree consumable)
            {
                cropsItemInstances.Add(new CropsInstance(consumable, invSlot.ItemInstance.Quantity));
            }
        }

        // Tạo UI Slot mới dựa trên dữ liệu đã lọc
        foreach (var vegetableItem in cropsItemInstances)
        {
            CropToolbarSlot cropSlot = pooler.GetNew();

            cropSlot.gameObject.name = vegetableItem.ItemStaticData.name;

            cropSlot.SetupCropToolbarSlot(vegetableItem);
            uiCropSlots.Add(cropSlot);
        }

    }
}
