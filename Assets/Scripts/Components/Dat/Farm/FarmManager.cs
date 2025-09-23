using FarmSystem;
using System.Collections.Generic;
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
    public TutorialManager TutorialManager;
    public CharacterPlayer CharacterPlayer;
    public List<Soil> Soils;

    private void Start()
    {
        // Load trạng thái nông trại khi vào scene
        FarmSaveSystem.Load(this);
    }
    public void RemoveCurrentTree()
    {
        CurrentSoilSelected.DestroyCurrentTree();
    }
}
