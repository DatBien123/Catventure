using System.Collections.Generic;
using UnityEngine;

public class CropToolbar : MonoBehaviour
{
    public CropToolbarSlot CropSlotToolbarPrefab;
    public Transform CropToolbarTransformParent;
    public List<CropToolbarSlot> uiCropSlots = new List<CropToolbarSlot>();

    [Header("References")]
    public FarmManager FarmManager;
    public GameObject OutofStockNotify;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 10;
    protected ObjectPooler<CropToolbarSlot> pooler { get; private set; }
    #endregion

    private void Awake()
    {
        pooler = new ObjectPooler<CropToolbarSlot>();
        pooler.Initialize(this, poolCount, CropSlotToolbarPrefab, CropToolbarTransformParent);
    }
    private void OnEnable()
    {
        RefreshCropsToolbar();
    }
    private void Start()
    {
        RefreshCropsToolbar();
    }
    public void RefreshCropsToolbar()
    {
        // Trả slot cũ về pool
        foreach (var slot in uiCropSlots)
        {
            pooler.Free(slot);
        }
        uiCropSlots.Clear();

        // Lấy toàn bộ Crops trong inventory
        List<CropsInstance> cropsItemInstances = new List<CropsInstance>();

        foreach (InventorySlot invSlot in FarmManager.CharacterPlayer.Inventory.slots)
        {
            if (invSlot.ItemInstance.ItemStaticData is SO_Tree consumable)
            {
                cropsItemInstances.Add(new CropsInstance(consumable, invSlot.ItemInstance.Quantity));
            }
        }

        // 👉 Sắp xếp theo tên (bảng chữ cái)
        cropsItemInstances.Sort((a, b) =>
            string.Compare(a.ItemStaticData.name, b.ItemStaticData.name, System.StringComparison.OrdinalIgnoreCase));

        // Tạo UI Slot mới theo thứ tự đã sắp xếp
        for (int i = 0; i < cropsItemInstances.Count; i++)
        {
            CropsInstance vegetableItem = cropsItemInstances[i];

            CropToolbarSlot cropSlot = pooler.GetNew();
            cropSlot.gameObject.name = vegetableItem.ItemStaticData.name;
            cropSlot.SetupCropToolbarSlot(vegetableItem);

            // Đặt thứ tự trong Hierarchy
            cropSlot.transform.SetSiblingIndex(i);

            uiCropSlots.Add(cropSlot);
        }

        if(cropsItemInstances.Count <= 0)
        {
            OutofStockNotify.gameObject.SetActive(true);
        }
        else OutofStockNotify.gameObject.SetActive(false);
    }

}
