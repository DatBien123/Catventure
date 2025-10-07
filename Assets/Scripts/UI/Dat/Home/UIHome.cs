using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIHome : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public AudioManager audioManager;
    public UIMapDescription UIMapDescription;
    public GameObject MapLockedNotify;
    public TutorialManager TutorialManager;
    public MapManager MapManager;
    public UIUnlockMapNotify UUnlockMapNotify;

    public RectTransform[] Maps;

    public float DelayTime = 1.2f;


    private void OnEnable()
    {
        animator.CrossFadeInFixedTime("Home Open", 0.0f);
        StartCoroutine(DelaySound());

        AnimateMaps(); // 👈 gọi hiệu ứng cho các Map


        //Map Saved
        // Kiểm tra xem có map cũ và có quay về từ minigame không

        if(MapManager != null)
        {
            for (int i = 0; i < MapManager.ListMapTile.Count; i++)
            {
                if (MapManager.ListMapTile[i].MapInstance.isUnlock == true && MapManager.ListMapTile[i].MapInstance.isPlayUnlock == false)
                    return;
            }
        }

        MapInstance mapSelected = MapSaveSystem.GetSelected();

        if (GameSession.Instance.LastSceneName == "Minigame_Drag&DropCooking" ||
            GameSession.Instance.LastSceneName == "FixSword" ||
            GameSession.Instance.LastSceneName == "DongSonDrumPattern" ||
            GameSession.Instance.LastSceneName == "WordsSpyGameScene" ||
            GameSession.Instance.LastSceneName == "Minigame_FITB") // 👈 tên scene minigame của bạn
        {
            UIMapDescription.gameObject.SetActive(true);
            UIMapDescription.UIMapDetail.gameObject.SetActive(true);
            UIMapDescription.SetupMapDescription(mapSelected);
            UIMapDescription.MapDescription.SetActive(false);
            UIMapDescription.UIMapDetail.SetupMapDetail();
        }

        // 👇 Reset lại để lần sau từ scene khác không bị bật sai
        //GameSession.Instance.LastSceneName = null;
    }

    void HighlightSelectedMap(string mapName)
    {
        Debug.Log("Map cũ: " + mapName);
        // Gắn highlight map nếu cần
    }

    IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(DelayTime);

        if(audioManager != null)
        {
            audioManager.PlaySFX("Background Music");
        }
    }
    void AnimateMaps()
    {
        foreach (var map in Maps)
        {
            if (map == null) continue;

            // Ghi nhớ vị trí & scale ban đầu để dễ reset
            Vector3 startPos = map.anchoredPosition;
            Vector3 startScale = map.localScale;

            // ✨ Di chuyển lên xuống nhẹ (ví dụ 15px)
            map.DOAnchorPosY(startPos.y + 15f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetDelay(Random.Range(0f, 0.5f))
                .SetLoops(-1, LoopType.Yoyo); // -1 = vô hạn, Yoyo = đi lên rồi quay lại

            // ✨ Scale nhẹ lên xuống (ví dụ phóng to 1.05 lần)
            map.DOScale(startScale * 1.05f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
