using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMidAutumn : MonoBehaviour
{
    public Image Layer;
    public RectTransform[] Decorations;
    //public RectTransform[] MiniGames;
    public AudioManager AudioManager;

    // 📦 Biến lưu trạng thái ban đầu
    private Vector3[] miniGameStartScales;
    private Vector2[] miniGameStartPositions;

    private void Awake()
    {
        //// 📦 Ghi nhớ scale & vị trí ban đầu của MiniGames
        //if (MiniGames != null && MiniGames.Length > 0)
        //{
        //    miniGameStartScales = new Vector3[MiniGames.Length];
        //    miniGameStartPositions = new Vector2[MiniGames.Length];

        //    for (int i = 0; i < MiniGames.Length; i++)
        //    {
        //        if (MiniGames[i] == null) continue;
        //        miniGameStartScales[i] = MiniGames[i].localScale;
        //        miniGameStartPositions[i] = MiniGames[i].anchoredPosition;
        //    }
        //}
    }

    private void OnEnable()
    {
        AnimateMaps();

        if (AudioManager != null)
        {
            AudioManager.StopAllMusic();
            AudioManager.PlaySFX("Mid Autumn");
        }
    }

    private void OnDisable()
    {
        if (AudioManager != null)
        {
            AudioManager.StopAllMusic();
            AudioManager.PlaySFX("Background Music");
        }

        // ✨ Reset alpha Layer
        if (Layer != null)
        {
            Color c = Layer.color;
            c.a = 1f;
            Layer.color = c;
        }

        //// ✨ Reset lại MiniGames về trạng thái ban đầu
        //if (MiniGames != null)
        //{
        //    for (int i = 0; i < MiniGames.Length; i++)
        //    {
        //        if (MiniGames[i] == null) continue;

        //        MiniGames[i].DOKill(); // ❗ Dừng tất cả tween đang chạy
        //        MiniGames[i].localScale = miniGameStartScales[i];
        //        MiniGames[i].anchoredPosition = miniGameStartPositions[i];
        //    }
        //}
    }

    void AnimateMaps()
    {
        // ✨ Làm mờ Layer từ alpha 1 → 0 trong 3 giây
        if (Layer != null)
        {
            Layer.DOFade(0f, 3f)
                .SetEase(Ease.InOutSine);
        }

        // ✨ Decorations lơ lửng
        foreach (var map in Decorations)
        {
            if (map == null) continue;

            Vector3 startPos = map.anchoredPosition;
            Vector3 startScale = map.localScale;

            map.DOAnchorPosY(startPos.y + 15f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetDelay(Random.Range(0f, 0.5f))
                .SetLoops(-1, LoopType.Yoyo);

            map.DOScale(startScale * 1.05f, 1.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        //// ✨ MiniGames zoom rồi lơ lửng
        //for (int i = 0; i < MiniGames.Length; i++)
        //{
        //    RectTransform mini = MiniGames[i];
        //    if (mini == null) continue;

        //    Vector3 originalScale = miniGameStartScales[i];
        //    Vector2 originalPos = miniGameStartPositions[i];

        //    mini.DOScale(originalScale * 1.2f, 0.8f)
        //        .SetEase(Ease.OutBack)
        //        .OnComplete(() =>
        //        {
        //            mini.DOAnchorPosY(originalPos.y + Random.Range(10f, 20f), Random.Range(1.2f, 1.8f))
        //                .SetEase(Ease.InOutSine)
        //                .SetDelay(Random.Range(0f, 0.6f))
        //                .SetLoops(-1, LoopType.Yoyo);
        //        });
        //}
    }
}
