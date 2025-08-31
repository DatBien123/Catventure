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
    public Transform TrouserTransformRoot;
    public Transform ShoeLeftTransformRoot;
    public Transform ShoeRightTransformRoot;

    [Header("Hat Transform Set")]
    public List<Transform> HatTransforms;

    [Header("Shirt Transform Set")]
    public List<Transform> ShirtTransforms;

    [Header("Trouser Transform Set")]
    public List<Transform> TrouserTransforms;

    [Header("Shoes Transform Set")]
    public List<Transform> ShoesLeftTransforms;
    public List<Transform> ShoesRightTransforms;

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
            case ItemType.Shoes:
                //Turn off all hat
                foreach (Transform availableTransform in ShoesLeftTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                foreach (Transform availableTransform in ShoesRightTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                ShoeLeftTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                ShoeRightTransformRoot.Find(OutfitName).gameObject.SetActive(true);
                break;
            case ItemType.Trouser:
                //Turn off all hat
                foreach (Transform availableTransform in TrouserTransforms)
                {
                    availableTransform.gameObject.SetActive(false);
                }
                //Active Hat by Name
                TrouserTransformRoot.Find(OutfitName).gameObject.SetActive(true);
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
            case ItemType.Shoes:

                //Active Hat by Name
                ShoeLeftTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                ShoeRightTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                break;
            case ItemType.Trouser:

                //Active Hat by Name
                TrouserTransformRoot.Find(OutfitName).gameObject.SetActive(false);
                break;
        }
    }

}
