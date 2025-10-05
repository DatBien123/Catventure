using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIMapDescription : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image Image;

    [Header("Data")]
    public MapInstance CurrentMapInstanceSelected;

    [Header("Buttons")]
    public Button Explore_Button;

    [Header("References")]
    public UIMapDetail UIMapDetail;
    public GameObject MapDescription;
    public LoadingBarProgress LoadingBarProgress;

    private void Awake()
    {
        Explore_Button.onClick.AddListener(() => Explore());
    }

    public void SetupMapDescription(MapInstance mapInstance)
    {
        CurrentMapInstanceSelected = mapInstance;

        Name.text = mapInstance.MapData.Data.Name;
        Description.text = mapInstance.MapData.Data.description;
        Image.sprite = mapInstance.MapData.Data.Image;

        // Lưu trạng thái khi thiết lập map description
        MapManager mapManager = FindObjectOfType<MapManager>();
        MapSaveSystem.Save(mapManager.ListMapTile.Select(tile => tile.MapInstance).ToList());
    }

    public void Explore()
    {
        UIMapDetail.gameObject.SetActive(true);
        UIMapDetail.SetupMapDetail();
        MapDescription.SetActive(false);
    }
}