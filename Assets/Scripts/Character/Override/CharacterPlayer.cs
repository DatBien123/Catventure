using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : Character
{
    [Header("Outfit Items")]
    public OutfitInstance Hat;
    public OutfitInstance Shirt;
    public OutfitInstance Trouser;
    public OutfitInstance Shoes;

    [Header("Equip Socket")]
    public Transform HatTransformRoot;
    public Transform ShirtTransformRoot;
    public Transform GlassesTransformRoot;
    public Transform HandStuffTransformRoot;

    [Header("Hat Transform Set")]
    public List<Transform> HatTransforms;

    [Header("Shirt Transform Set")]
    public List<Transform> ShirtTransforms;

    [Header("Glasses Transform Set")]
    public List<Transform> GlassesTransforms;

    [Header("HandStuff Transform Set")]
    public List<Transform> HandStuffTransforms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }

}
