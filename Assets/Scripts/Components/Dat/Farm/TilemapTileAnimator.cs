using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class TilemapTileAnimator : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapRenderer renderer;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        renderer = GetComponent<TilemapRenderer>();

        // Ví dụ 1: Fade in Tilemap khi start (từ alpha 0 đến 1 trong 2 giây)
        renderer.material.color = new Color(1, 1, 1, 0); // Bắt đầu trong suốt
        renderer.material.DOFade(1, 2f).SetEase(Ease.OutQuad);

        // Ví dụ 2: Di chuyển Tilemap lên xuống liên tục (loop)
        transform.DOMoveY(transform.position.y + 1f, 1f)
                 .SetRelative()
                 .SetLoops(-1, LoopType.Yoyo) // Loop vô tận, yoyo (lên xuống)
                 .SetEase(Ease.InOutSine);

        // Ví dụ 3: Scale Tilemap (phóng to/thu nhỏ)
        transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f)
                 .SetLoops(3, LoopType.Yoyo)
                 .SetEase(Ease.OutBounce);

        // Ví dụ 4: Rung lắc (punch) khi cần (gọi từ hàm khác)
        // transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 10, 1);
    }

    // Hàm ví dụ để trigger animation từ event (như va chạm)
    public void ShakeTilemap()
    {
        transform.DOPunchPosition(Vector3.right * 0.5f, 0.3f, 5, 1);
    }
}
