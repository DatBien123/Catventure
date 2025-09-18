using FarmSystem;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Soil CurrentSoilSelected;

    public Sprite ScytheSprite;
    public Sprite CanSprite;
    public Sprite PickaxeSprite;

    [Header("UI References")]
    public CropToolbar CropsToolbar;
    public GameObject HavestToolbar;
    public GameObject RemoveToolbar;
    public GameObject WateringToolbar;
    public GameObject RestorationToolbar;

    [Header("References")]
    public CharacterPlayer CharacterPlayer;

    public void RemoveCurrentTree()
    {
        CurrentSoilSelected.DestroyCurrentTree();
    }
}
