using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class UIMapTile : MonoBehaviour
{
    [Header("Components")]
    public Button Map_Button;
    public Image Map_Image;
    public Image LockImage;
    public Color UnlockColor;
    public Color LockColor;
    public GameObject MapName;
    public string name;

    [Header("References")]
    public UIHome UIHome;

    [Header("Data")]
    public MapInstance MapInstance;

    private void Awake()
    {
        Map_Button.onClick.AddListener(() => SelectMap());
    }

    public void SetupMapTile(bool isUnlock)
    {
        if (isUnlock)
        {


            if (!MapInstance.isPlayUnlock)
            {
                PlayUnlockMap();
            }
            else
            {
                if (LockImage != null)
                    LockImage.gameObject.SetActive(false);
                Map_Image.color = UnlockColor;
                MapName.SetActive(true);
            }
        }
        else
        {
            if (LockImage != null)
                LockImage.gameObject.SetActive(true);
            Map_Image.color = LockColor;
            MapName.SetActive(false);
        }


    }
    public void SelectMap()
    {


        if (!MapInstance.isUnlock)
        {
            StartCoroutine(ShowLockedNotify());
            return;
        }

        if (UIHome.TutorialManager.currentPart.TutorialName == "Home Adventure" && UIHome.TutorialManager.currentStep.stepName == "HaNoi Tap")
        {
            UIHome.TutorialManager.AllowNextStep = true;
        }


        //GameSession.Instance.SetLastSelectedMap(name);

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

    Coroutine C_PlayUnlockMap;
    public void PlayUnlockMap()
    {
        if (MapInstance.isUnlock && !MapInstance.isPlayUnlock)
        {
            if (C_PlayUnlockMap != null) StopCoroutine(C_PlayUnlockMap);
            C_PlayUnlockMap = StartCoroutine(Coroutine_PlayUnlockMap());
        }

    }

    IEnumerator Coroutine_PlayUnlockMap()
    {
        // Disable all buttons trong lúc chơi animation
        DisableAllButtons();

        //Map_Button.interactable = true;

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Play Unlock");
        LockImage.gameObject.SetActive(true);

        // Bước 1: Nếu có LockImage thì rung và co nhỏ dần
        if (LockImage != null && LockImage.gameObject.activeSelf)
        {
            RectTransform rect = LockImage.GetComponent<RectTransform>();
            rect.localScale = Vector3.one; // reset scale

            // Rung nhẹ (shake)
            rect.DOShakeScale(0.5f, strength: 0.3f, vibrato: 10, randomness: 0, fadeOut: true);
            yield return new WaitForSeconds(0.5f);

            // Co nhỏ dần
            rect.DOScale(Vector3.zero, 0.8f)
                .SetEase(Ease.InBack)
                .OnComplete(() => LockImage.gameObject.SetActive(false));

            yield return new WaitForSeconds(0.8f);
        }

        // Bước 2: Đổi màu từ LockColor sang UnlockColor
        Map_Image.color = LockColor;
        Map_Image.DOColor(UnlockColor, 1.5f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(1.5f);

        // Hiện tên map nếu có
        if (MapName != null)
            MapName.SetActive(true);

        // Reset cờ để không lặp lại animation
        MapInstance.isPlayUnlock = true;

        // Cập nhật trạng thái map trong MapManager
        MapManager mapManager = FindObjectOfType<MapManager>();
        foreach (var tile in mapManager.ListMapTile)
        {
            if (tile.MapInstance.MapData.name == MapInstance.MapData.name)
            {
                tile.MapInstance.isPlayUnlock = true;
                break;
            }
        }
        MapSaveSystem.Save(mapManager.ListMapTile.Select(tile => tile.MapInstance).ToList());

        EnableAllButtons();

        // Notify
        UIHome.UUnlockMapNotify.SetupUIUnlockMapNotify(MapInstance);
        UIHome.UUnlockMapNotify.gameObject.SetActive(true);
    }


    public void EnableAllButtons()
    {
        foreach (Button btn in FindObjectsOfType<Button>(true))
        {
            btn.interactable = true;
        }
    }
    public void DisableAllButtons()
    {
        foreach (Button btn in FindObjectsOfType<Button>(true))
        {
            btn.interactable = false;

        }
    }


}