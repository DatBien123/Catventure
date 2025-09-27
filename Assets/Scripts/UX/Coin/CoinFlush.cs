using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CoinFlush : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform moneyCounter; // Kéo UI counter vào đây
    public Coin coinPrefab; // Prefab coin Image
    //public int numCoins = 10; // Số coin rơi
    public Vector2 spawnArea = new Vector2(800, 600); // Vùng spawn (pixels, tùy resolution)
    public float fallDuration = 1f; // Thời gian rơi
    public float flyDuration = 0.5f; // Thời gian bay vào counter
    public float delayBetweenCoins = 0.1f; // Delay spawn mỗi coin

    private Canvas canvas;

    #region [ Pool ]
    [SerializeField] protected int poolCount = 10;
    protected ObjectPooler<Coin> pooler { get; private set; }
    //protected GameObject poolParent;

    #endregion

    private void Awake()
    {
        pooler = new ObjectPooler<Coin>();
        pooler.Initialize(this, poolCount, coinPrefab, transform);
    }
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        // Gọi effect khi cần (ví dụ: từ script khác)
        // StartCoinEffect();
    }
    public void StartCoinEffect(int numCoins)
    {
        StartCoroutine(SpawnAndAnimateCoins(numCoins));
    }

    public void StopCoinEffect()
    {
        foreach (Transform child in transform)
        {
            pooler.Free(child.GetComponent<Coin>());
        }
    }
    private IEnumerator SpawnAndAnimateCoins(int numCoins)
    {
        numCoins = Mathf.Clamp(numCoins, 1, 10); 
        for (int i = 0; i < numCoins; i++)
        {
            // Spawn coin tại vị trí ngẫu nhiên cao (rơi từ trên)
            RectTransform coinRect = pooler.GetNew().GetComponent<RectTransform>();
            coinRect.localScale = Vector3.one;

            Vector2 spawnPos = new Vector2(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                Random.Range(spawnArea.y / 2, spawnArea.y) // Cao để rơi xuống
            );
            coinRect.anchoredPosition = spawnPos;

            // Animate rơi xuống (di chuyển y xuống với easing)
            StartCoroutine(AnimateFall(coinRect, fallDuration));

            yield return new WaitForSeconds(delayBetweenCoins);
        }


    }

    private IEnumerator AnimateFall(RectTransform coin, float duration)
    {
        Vector2 startPos = coin.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, -spawnArea.y / 2); // Rơi xuống dưới màn hình

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Easing cho rơi tự nhiên (ease out)
            t = 1 - Mathf.Pow(1 - t, 3); // Cubic ease out
            coin.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Sau khi rơi xong, bay vào counter
        StartCoroutine(AnimateFlyToCounter(coin, flyDuration));
    }

    private IEnumerator AnimateFlyToCounter(RectTransform coin, float duration)
    {
        Vector2 targetPos = moneyCounter.anchoredPosition; // Vị trí counter
        Vector2 startPos = coin.anchoredPosition;

        // Tùy chọn: Scale nhỏ dần khi bay
        Vector3 startScale = coin.localScale;
        Vector3 endScale = startScale * 0.5f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // Smooth easing
            coin.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            coin.localScale = Vector3.Lerp(startScale, endScale, t);

            // Tùy chọn: Rotate coin
            coin.Rotate(0, 0, 360 * Time.deltaTime); // Xoay liên tục

            yield return null;
        }

        // Xóa coin sau khi bay xong
        pooler.Free(coin.GetComponent<Coin>());

        // Chạy animation zoom cho counter
        StartCoroutine(AnimateCounterZoom());

        // Cập nhật counter (ví dụ: tăng tiền)
        //UpdateMoneyCounter(1); // Thay bằng logic thực
    }
    private IEnumerator AnimateCounterZoom()
    {
        float zoomDuration = 0.3f; // Thời gian zoom
        Vector3 originalScale = Vector3.one; // Lưu scale gốc
        Vector3 zoomScale = originalScale * 1.2f; // Zoom to 120%

        // Zoom to lớn
        float elapsed = 0f;
        while (elapsed < zoomDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (zoomDuration / 2);
            t = Mathf.SmoothStep(0f, 1f, t); // Smooth easing
            moneyCounter.localScale = Vector3.Lerp(originalScale, zoomScale, t);
            yield return null;
        }

        // Thu nhỏ về kích thước ban đầu
        elapsed = 0f;
        while (elapsed < zoomDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (zoomDuration / 2);
            t = Mathf.SmoothStep(0f, 1f, t); // Smooth easing
            moneyCounter.localScale = Vector3.Lerp(zoomScale, originalScale, t);
            yield return null;
        }

        // Đảm bảo scale chính xác về gốc
        moneyCounter.localScale = originalScale;
    }

    //private void UpdateMoneyCounter(int amount)
    //{
    //    // Giả sử counter là Text
    //    Text counterText = moneyCounter.GetComponent<Text>();
    //    if (counterText != null)
    //    {
    //        int current = int.Parse(counterText.text.Replace("Coins: ", ""));
    //        counterText.text = "Coins: " + (current + amount);
    //    }
    //}
}
