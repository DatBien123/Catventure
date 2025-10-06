using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPlayer : Character
{
    [Header("Data")]
    public int Coin = 100000;
    public int CurrentEnergy = 100;
    public int MaxEnergy = 300;
    public bool isFirstTimeLogin = true;

    public Inventory Inventory;

    [Header("Outfit Items")]
    public OutfitInstance Hat;
    public OutfitInstance Shirt;
    public OutfitInstance HandStuff;
    public OutfitInstance Glasses;
    public OutfitInstance Wing;

    [Header("Equip Socket")]
    public Transform HatTransformRoot;
    public Transform ShirtTransformRoot;
    public Transform GlassesTransformRoot;
    public Transform HandStuffTransformRoot;
    public Transform WingTransformRoot;

    [Header("Hat Transform Set")]
    public List<Transform> HatTransforms;

    [Header("Shirt Transform Set")]
    public List<Transform> ShirtTransforms;

    [Header("Glasses Transform Set")]
    public List<Transform> GlassesTransforms;

    [Header("HandStuff Transform Set")]
    public List<Transform> HandStuffTransforms;

    [Header("Wing Transform Set")]
    public List<Transform> WingTransforms;


    [Header("Initialized Data")]
    public List<ItemInstance> ItemsInitialized;
    protected override void Awake()
    {
        base.Awake();
        // Khởi tạo các outfit mặc định nếu chưa có
        //if (Hat == null) Hat = null; // Hoặc gán một OutfitInstance mặc định
        //if (Shirt == null) Shirt = null;
        //if (Glasses == null) Glasses = null;
        //if (HandStuff == null) HandStuff = null;
        //if (Wing == null) Wing = null;

        SaveSystem.Load(this, Inventory);

        if (isFirstTimeLogin)
        {
            foreach (var Item in ItemsInitialized) {
                Inventory.AddItem(Item);
            }
        }


    }

    void InitializedDataValue()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Wear(ItemType itemType, string OutfitName)
    {
        switch (itemType)
        {
            case ItemType.Hat:
                //Turn off all hat
                foreach (Transform availableTransform in HatTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                HatTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                break;
            case ItemType.Shirt:
                //Turn off all hat
                foreach (Transform availableTransform in ShirtTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                ShirtTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                break;
            case ItemType.Glasses:
                //Turn off all hat
                foreach (Transform availableTransform in GlassesTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                GlassesTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                break;
            case ItemType.HandStuff:
                //Turn off all hat
                foreach (Transform availableTransform in HandStuffTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                HandStuffTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                if(HandStuffTransformRoot.Find(OutfitName).gameObject.GetComponent<Animator>())
                    HandStuffTransformRoot.Find(OutfitName).gameObject.GetComponent<Animator>().CrossFadeInFixedTime(OutfitName + "_Active", .0f);

                break;
            case ItemType.Wing:
                //Turn off all hat
                foreach (Transform availableTransform in WingTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                WingTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                if (WingTransformRoot.Find(OutfitName).gameObject.GetComponent<Animator>())
                    WingTransformRoot.Find(OutfitName).gameObject.GetComponent<Animator>().CrossFadeInFixedTime(OutfitName + "_Active", .0f);
                break;
        }

    }
    public void TakeOff(ItemType itemType, string OutfitName) 
    {
        switch (itemType)
        {
            case ItemType.Hat:

                //Active Hat by Name
                HatTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                break;
            case ItemType.Shirt:

                //Active Hat by Name
                ShirtTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                break;
            case ItemType.Glasses:

                //Active Hat by Name
                GlassesTransformRoot.Find(OutfitName).gameObject.SetActive(false);

                break;
            case ItemType.HandStuff:

                //Active Hat by Name
                HandStuffTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                break;
            case ItemType.Wing:

                //Active Hat by Name
                WingTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                break;
        }
    }

    public bool IsOutfitItemActive(ItemType itemType, string OutfitName)
    {
        bool result = false;

        switch (itemType)
        {
            case ItemType.Hat:
                
                result = HatTransformRoot.Find(OutfitName).gameObject.activeInHierarchy ? true : false;

                break;
            case ItemType.Shirt:

                result = ShirtTransformRoot.Find(OutfitName).gameObject.activeInHierarchy ? true : false;

                break;
            case ItemType.Glasses:

                result = GlassesTransformRoot.Find(OutfitName).gameObject.activeInHierarchy ? true : false;

                break;
            case ItemType.HandStuff:

                result = HandStuffTransformRoot.Find(OutfitName).gameObject.activeInHierarchy ? true : false;

                break;
            case ItemType.Wing:

                result = WingTransformRoot.Find(OutfitName).gameObject.activeInHierarchy ? true : false;

                break;
        }

        return result;
    }

}
