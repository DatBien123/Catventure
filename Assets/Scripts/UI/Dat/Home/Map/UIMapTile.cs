using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections; // ⚠️ nhớ import DOTween

public class UIMapTile : MonoBehaviour
{
    [Header("Components")]
    public Button Map_Button;

    [Header("References")]
    public UIHome UIHome;

    [Header("Data")]
    public SO_Map MapData;

    private void Awake()
    {
        Map_Button.onClick.AddListener(() => SelectMap());
    }

    public void SelectMap()
    {
        if (!MapData.Data.isUnlock)
        {
            StartCoroutine(ShowLockedNotify());
            return;
        }

        UIHome.UIMapDescription.SetupMapDescription(MapData);
        UIHome.UIMapDescription.gameObject.SetActive(true);
        UIHome.audioManager.PlaySFX("Click Map Home");
    }

    private IEnumerator ShowLockedNotify()
    {
        GameObject notifyObj = UIHome.MapLockedNotify.gameObject;

        // Nếu notify đang hiện rồi thì reset animation lại
        notifyObj.SetActive(true);

        RectTransform rect = notifyObj.GetComponent<RectTransform>();

        // Reset vị trí gốc để rung từ trung tâm
        Vector3 originalPos = rect.anchoredPosition;
        rect.anchoredPosition = originalPos;

        // Nếu có CanvasGroup thì fade in
        CanvasGroup cg = notifyObj.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0;
            cg.DOFade(1, 0.2f);
        }

        // Hiệu ứng rung trái phải 3 lần trong 0.5 giây
        rect.DOShakeAnchorPos(0.5f, strength: new Vector2(20, 0), vibrato: 10, randomness: 0, snapping: false, fadeOut: true);

        // Thêm âm thanh nếu muốn
        UIHome.audioManager.PlaySFX("Map Locked Notify");

        // Chờ vài giây rồi ẩn notify đi
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
