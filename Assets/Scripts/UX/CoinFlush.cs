using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CoinFlush : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform moneyCounter; // Kéo UI counter vào đây
    public GameObject coinPrefab; // Prefab coin Image
    public int numCoins = 10; // Số coin rơi
    public Vector2 spawnArea = new Vector2(800, 600); // Vùng spawn (pixels, tùy resolution)
    public float fallDuration = 1f; // Thời gian rơi
    public float flyDuration = 0.5f; // Thời gian bay vào counter
    public float delayBetweenCoins = 0.1f; // Delay spawn mỗi coin

    private Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        // Gọi effect khi cần (ví dụ: từ script khác)
        // StartCoinEffect();
    }

    public void StartCoinEffect()
    {
        StartCoroutine(SpawnAndAnimateCoins());
    }

    private IEnumerator SpawnAndAnimateCoins()
    {
        for (int i = 0; i < numCoins; i++)
        {
            // Spawn coin tại vị trí ngẫu nhiên cao (rơi từ trên)
            RectTransform coinRect = Instantiate(coinPrefab, transform).GetComponent<RectTransform>();
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
        Destroy(coin.gameObject);

        // Cập nhật counter (ví dụ: tăng tiền)
        //UpdateMoneyCounter(1); // Thay bằng logic thực
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
