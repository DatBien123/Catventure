using FarmSystem;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Soil CurrentSoilSelected;

    public void PlanTreeToCurrentSoil(string treeName)
    {
        CurrentSoilSelected.PlantTree(treeName);
    }
}
