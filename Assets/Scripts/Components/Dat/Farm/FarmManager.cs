using FarmSystem;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [Header("References")]
    public Soil CurrentSoilSelected;
    public Sprite ScytheSprite;

    [Header("UI References")]
    public GameObject CropsToolbar;
    public GameObject HavestToolbar;
    public GameObject RemoveToolbar;

    public void RemoveCurrentTree()
    {
        CurrentSoilSelected.DestroyCurrentTree();
    }

}
