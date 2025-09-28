using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public enum FilterType
{
    Shirt,
    Glasses,
    HandStuff,
    Hat,
    Consumable,
    Wing,
    Crops

}

public class UIInventory : MonoBehaviour
{
    public Inventory inventoryManager;

    [Header("UI")]
    public UIInventorySlot slotPrefab;
    public Transform slotParent;

    [Header("Filter Buttons")]
    public Button buttonShirt;
    public Button buttonHandStuff;
    public Button buttonGlasses;
    public Button buttonHat;
    public Button buttonWing;
    public Button buttonConsumable;
    public Button buttonCrops;

    [Header("Interact Button")]
    public Button buttonWear;
    public Button buttonTakeoff;

    [Header("Data")]
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Coin;

    public List<UIInventorySlot> uiSlots = new List<UIInventorySlot>();

    public UIInventorySlot currentInventorySlotSelected;
    public UIItemDetail UIItemDetail;

    public FilterType currentFilter = FilterType.Shirt;

    public UnityEvent OnWear;
    public UnityEvent OnTakeOff;

    [Header("References")]
    public CoinFlush CoinFlush;
    public CoinFlush EnegyFlush;
    public UIYabis UIYabis;
    public UIFarm UIFarm;
    public AudioManager AudioManager;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 100;
    protected ObjectPooler<UIInventorySlot> pooler { get; private set; }
    //protected GameObject poolParent;

    #endregion

    private void OnEnable()
    {
        RefreshUI();
    }

    private void OnDisable()
    {
        if(CoinFlush != null)
        CoinFlush.StopCoinEffect();
        if (EnegyFlush != null)
        EnegyFlush.StopCoinEffect();
    }
    private void Awake()
    {
        pooler = new ObjectPooler<UIInventorySlot>();
        pooler.Initialize(this, poolCount, slotPrefab, slotParent);
    }

    private void Start()
    {
        //Filter Register
        if(buttonShirt != null) buttonShirt.onClick.AddListener(() => ChangeFilter(FilterType.Shirt));
        if (buttonGlasses != null) buttonGlasses.onClick.AddListener(() => ChangeFilter(FilterType.Glasses));
        if (buttonHandStuff != null) buttonHandStuff.onClick.AddListener(() => ChangeFilter(FilterType.HandStuff));
        if (buttonHat != null) buttonHat.onClick.AddListener(() => ChangeFilter(FilterType.Hat));
        if (buttonConsumable != null) buttonConsumable.onClick.AddListener(() => ChangeFilter(FilterType.Consumable));
        if (buttonWing != null) buttonWing.onClick.AddListener(() => ChangeFilter(FilterType.Wing));
        if (buttonCrops != null) buttonCrops.onClick.AddListener(() => ChangeFilter(FilterType.Crops));

        //Take off - Wear Register

        if (buttonTakeoff != null) buttonTakeoff.onClick.AddListener(() => TakeOff(currentInventorySlotSelected.slotData.ItemInstance));
        if (buttonWear != null) buttonWear.onClick.AddListener(() => Wear(currentInventorySlotSelected.slotData.ItemInstance));

        RefreshUI();

        if (buttonTakeoff != null) buttonTakeoff.gameObject.SetActive(false);
        if (buttonWear != null) buttonWear.gameObject.SetActive(false);

    }
    #region [Filter]
    public void ChangeFilter(FilterType filter)
    {
        if(AudioManager != null)
        {
            AudioManager.PlaySFX("Filter");
        }
        currentFilter = filter;
        RefreshUI();
    }
    #endregion

    #region [Outfit Action]
    public void ShowActionButton(ItemInstance ItemToWear)
    {
        if(ItemToWear.ItemStaticData.commonData.itemType != ItemType.Consumable && ItemToWear.ItemStaticData.commonData.itemType != ItemType.Crops)
        {
            //Case 1: Outfit Equiped
            if (/*ItemToWear.IsEquiped && */ inventoryManager.owner.IsOutfitItemActive(ItemToWear.ItemStaticData.commonData.itemType, ItemToWear.ItemStaticData.commonData.itemName))
            {
                if(buttonTakeoff != null)
                    buttonTakeoff.gameObject.SetActive(true);
                if (buttonWear != null)
                    buttonWear.gameObject.SetActive(false);
            }
            else
            {
                if (buttonWear != null)
                    buttonWear.gameObject.SetActive(true);
                if (buttonTakeoff != null)
                    buttonTakeoff.gameObject.SetActive(false);
            }
            //Case 2: Outfit Unequiped
        }
        else
        {
            //Only Now (Test)
            if (buttonTakeoff != null)
                buttonTakeoff.gameObject.SetActive(false);
            if (buttonWear != null)
                buttonWear.gameObject.SetActive(false);

            UIItemDetail.gameObject.SetActive(true);
            UIItemDetail.SetupItemDetail(currentInventorySlotSelected.slotData.ItemInstance);
        }

    }
    public void Wear(ItemInstance ItemToWear)
    {
        if (AudioManager != null)
        {
            AudioManager.PlaySFX("Wear");
        }

        OnWear?.Invoke();
        //Luồng: Huỷ IsEquiped - Gán mới tham chiếu 
        if(ItemToWear.ItemStaticData as SO_Outfit)
        {
            Debug.Log("True");
        }
        Debug.Log("Item wear is: " + ItemToWear.ItemStaticData.commonData.itemName);

        //Huỷ Equiped tại Outfit hiện tại
        foreach(InventorySlot inventorySlot in inventoryManager.slots)
        {
                if(inventorySlot.ItemInstance.IsEquiped && inventorySlot.ItemInstance.ItemStaticData.commonData.itemType == ItemToWear.ItemStaticData.commonData.itemType)
                {
                    inventorySlot.ItemInstance.IsEquiped = false;
                }
        }

        if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Shirt)
        {
            inventoryManager.owner.Shirt = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true);
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Hat)
        {
            inventoryManager.owner.Hat = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true); ;
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Glasses)
        {
            inventoryManager.owner.Glasses = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true); ;
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.HandStuff)
        {
            inventoryManager.owner.HandStuff = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true);

            if ((ItemToWear.ItemStaticData as SO_Outfit).outfitData.equipClip != null)
                inventoryManager.owner.animator.CrossFadeInFixedTime((ItemToWear.ItemStaticData as SO_Outfit).outfitData.equipClip.name, 0.0f);
        }
        else if (ItemToWear.ItemStaticData.commonData.itemType == ItemType.Wing)
        {
            inventoryManager.owner.Wing = new OutfitInstance(ItemToWear.ItemStaticData as SO_Outfit, 1, true);

            if((ItemToWear.ItemStaticData as SO_Outfit).outfitData.equipClip != null)
            inventoryManager.owner.animator.CrossFadeInFixedTime((ItemToWear.ItemStaticData as SO_Outfit).outfitData.equipClip.name, 0.0f);
        }

        inventoryManager.owner.Wear(ItemToWear.ItemStaticData.commonData.itemType, ItemToWear.ItemStaticData.commonData.itemName);

        ItemToWear.IsEquiped = true;

        ShowActionButton(ItemToWear);

        RefreshUI();
        //Save Data
        SaveSystem.Save(inventoryManager.owner, inventoryManager);
    }
    public void TakeOff(ItemInstance ItemToTakeOff)
    {
        if (AudioManager != null)
        {
            AudioManager.PlaySFX("Take Off");
        }
        OnTakeOff?.Invoke();

        if (ItemToTakeOff.ItemStaticData.commonData.itemType == ItemType.Shirt)
        {
            inventoryManager.owner.Shirt = null;
        }
        else if (ItemToTakeOff.ItemStaticData.commonData.itemType == ItemType.Hat)
        {
            inventoryManager.owner.Hat = null;
        }
        else if (ItemToTakeOff.ItemStaticData.commonData.itemType == ItemType.Glasses)
        {
            inventoryManager.owner.Glasses = null;
        }
        else if (ItemToTakeOff.ItemStaticData.commonData.itemType == ItemType.HandStuff)
        {
            inventoryManager.owner.HandStuff = null;
        }
        else if (ItemToTakeOff.ItemStaticData.commonData.itemType == ItemType.Wing)
        {
            inventoryManager.owner.Wing = null;
        }

        ItemToTakeOff.IsEquiped = false;

        inventoryManager.owner.TakeOff(ItemToTakeOff.ItemStaticData.commonData.itemType, ItemToTakeOff.ItemStaticData.commonData.itemName);

        ShowActionButton(ItemToTakeOff);

        RefreshUI();

        //Save Data
        SaveSystem.Save(inventoryManager.owner, inventoryManager);
    }
    #endregion

    public void RefreshUI()
    {
        // Xoá hết UI Slot cũ
        foreach (var slot in uiSlots)
        {
            pooler.Free(slot);
        }
        uiSlots.Clear();

        // Lọc dữ liệu inventory theo filter
        var filteredSlots = inventoryManager.slots
            .Where(slot =>
                (currentFilter == FilterType.Shirt && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Shirt) ||
                (currentFilter == FilterType.Glasses && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Glasses) ||
                (currentFilter == FilterType.HandStuff && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.HandStuff) ||
                (currentFilter == FilterType.Hat && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Hat) ||
                (currentFilter == FilterType.Consumable && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Consumable) ||
                (currentFilter == FilterType.Wing && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Wing) ||
                (currentFilter == FilterType.Crops && slot.ItemInstance.ItemStaticData.commonData.itemType == ItemType.Crops)
            )
            // 👇 Sắp xếp theo giá tăng dần
            .OrderBy(slot => slot.ItemInstance.ItemStaticData.commonData.price);

        // Tạo UI Slot mới dựa trên dữ liệu đã lọc
        foreach (var invSlot in filteredSlots)
        {
            UIInventorySlot uiSlot = pooler.GetNew();
            uiSlot.slotData = invSlot;

            uiSlot.SetupSlot(invSlot);
            uiSlot.Outline.enabled = false;
            uiSlots.Add(uiSlot);
        }

        if (buttonTakeoff != null)
            buttonTakeoff.gameObject.SetActive(false);
        if (buttonWear != null)
            buttonWear.gameObject.SetActive(false);

        UpdateResourceUI();
    }


    public void UpdateResourceUI()
    {
        Energy.text = inventoryManager.owner.CurrentEnergy.ToString() + " / " + inventoryManager.owner.MaxEnergy.ToString();
        Coin.text = inventoryManager.owner.Coin.ToString();
    }
}
