using FarmSystem;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [Header("References")]
    public Soil CurrentSoilSelected;

    public Sprite ScytheSprite;
    public Sprite CanSprite;
    public Sprite PickaxeSprite;

    [Header("UI References")]
    public GameObject CropsToolbar;
    public GameObject HavestToolbar;
    public GameObject RemoveToolbar;
    public GameObject WateringToolbar;
    public GameObject RestorationToolbar;

    public void RemoveCurrentTree()
    {
        CurrentSoilSelected.DestroyCurrentTree();
    }

}
