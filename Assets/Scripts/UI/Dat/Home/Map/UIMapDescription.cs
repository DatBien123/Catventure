using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMapDescription : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image Image;

    [Header("Data")]
    public SO_Map CurrentMapSelected;
    public SO_MapSelecting MapSelecting;

    [Header("Buttons")]
    public Button Explore_Button;

    [Header("References")]
    public UIMapDetail UIMapDetail;
    public GameObject MapDescription;

    private void Awake()
    {
        Explore_Button.onClick.AddListener(() => Explore());
    }
    public void SetupMapDescription(SO_Map mapData)
    {
        CurrentMapSelected = mapData;
        MapSelecting.Data.CurrentMapSelecting = CurrentMapSelected;

        Name.text = mapData.Data.Name;
        Description.text = mapData.Data.description.ToString();
        Image.sprite = mapData.Data.Image;
    }

    public void Explore()
    {
        UIMapDetail.gameObject.SetActive(true);
        UIMapDetail.SetupMapDetail();
        MapDescription.SetActive(false);
    }
}
