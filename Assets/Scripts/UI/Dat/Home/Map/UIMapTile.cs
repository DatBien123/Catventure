using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class UIMapTile : MonoBehaviour
{
    [Header("Components")]
    public Button Map_Button;

    [Header("References")]
    public UIHome UIHome;

    [Header("Data")]
    public MapInstance MapInstance;

    private void Awake()
    {
        Map_Button.onClick.AddListener(() => SelectMap());
    }

    public void SelectMap()
    {
        if (!MapInstance.isUnlock)
        {
            StartCoroutine(ShowLockedNotify());
            return;
        }

        // Đặt tất cả map khác thành không chọn
        foreach (var tile in FindObjectsOfType<UIMapTile>())
        {
            tile.MapInstance.isSelected = false;
        }
        MapInstance.isSelected = true;

        // Lưu trạng thái sau khi chọn map
        MapManager mapManager = FindObjectOfType<MapManager>();
        MapSaveSystem.Save(mapManager.ListMapTile.Select(tile => tile.MapInstance).ToList());

        UIHome.UIMapDescription.SetupMapDescription(MapInstance);
        UIHome.UIMapDescription.gameObject.SetActive(true);
        UIHome.UIMapDescription.MapDescription.SetActive(true);
        UIHome.audioManager.PlaySFX("Click Map Home");
    }

    private IEnumerator ShowLockedNotify()
    {
        GameObject notifyObj = UIHome.MapLockedNotify.gameObject;

        // Nếu notify đang hiện thì reset animation
        notifyObj.SetActive(true);

        RectTransform rect = notifyObj.GetComponent<RectTransform>();

        // Reset vị trí gốc
        Vector3 originalPos = rect.anchoredPosition;
        rect.anchoredPosition = originalPos;

        // Fade in nếu có CanvasGroup
        CanvasGroup cg = notifyObj.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0;
            cg.DOFade(1, 0.2f);
        }

        // Hiệu ứng rung
        rect.DOShakeAnchorPos(0.5f, strength: new Vector2(20, 0), vibrato: 10, randomness: 0, snapping: false, fadeOut: true);

        UIHome.audioManager.PlaySFX("Map Locked Notify");

        // Chờ rồi ẩn notify
        yield return new WaitForSeconds(1.2f);

        if (cg != null)
        {
            cg.DOFade(0, 0.2f).OnComplete(() => notifyObj.SetActive(false));
        }
        else
        {
            notifyObj.SetActive(false);
        }
    }
}