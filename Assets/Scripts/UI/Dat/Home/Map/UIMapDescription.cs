using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMapDescription : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image Image;

    public SO_Map CurrentMapSelected;
    public SO_MapSelecting MapSelecting;

    public void SetupMapDescription(SO_Map mapData)
    {
        CurrentMapSelected = mapData;
        MapSelecting.Data.CurrentMapSelecting = CurrentMapSelected;

        Name.text = mapData.Data.Name;
        Description.text = mapData.Data.description.ToString();
        Image.sprite = mapData.Data.Image;
    }
}
