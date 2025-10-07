using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnlockMapNotify : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI Name;
    public Image Image;

    [Header("Data")]
    public MapInstance CurrentMapInstance;

    [Header("Animations")]
    public GameObject UnlockMapNotifyGO;

    [Header("References")]
    public UIHome UIHome;

    private void OnEnable()
    {
        UIHome.audioManager.PlaySFX("Story Showup");
        UnlockMapNotifyGO.transform.localScale = Vector3.zero; // Start from zero scale
        UnlockMapNotifyGO.transform.DOScale(1.1f, 0.3f) // Zoom to 1.1f slightly overshooting
            .SetEase(Ease.OutBack) // Adds a back effect for the overshoot
            .OnComplete(() => UnlockMapNotifyGO.transform.DOScale(1f, 0.2f)); // Settle back to 1f
    }

    public void SetupUIUnlockMapNotify(MapInstance mapInstance)
    {
        CurrentMapInstance = mapInstance;

        Name.text = mapInstance.MapData.Data.Name;
        Image.sprite = mapInstance.MapData.Data.Image;
    }

}
