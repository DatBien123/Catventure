using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIMainMenu : MonoBehaviour
{
    public RectTransform[] Maps;

    public VideoPlayer VideoPlayer;

    public void Awake()
    {

        VideoPlayer.Prepare();

        StartCoroutine(PreloadNextScene("Home Scene"));



    }

    private void Start()
    {

    }
    public AsyncOperation preloadSceneOp;

    public void ActiveHomeScene()
    {
        if (preloadSceneOp != null)
        {

            // 1️⃣ Cho phép kích hoạt scene Home
            preloadSceneOp.allowSceneActivation = true;

            StartCoroutine(SwitchToHomeAndUnloadMenu());
        }
    }

    private IEnumerator SwitchToHomeAndUnloadMenu()
    {
        // 2️⃣ Đợi scene Home kích hoạt hoàn toàn
        Scene homeScene = SceneManager.GetSceneByName("Home Scene");
        while (!homeScene.isLoaded)
            yield return null;

        // 3️⃣ Chuyển active scene sang Home
        SceneManager.SetActiveScene(homeScene);

        // 4️⃣ Bây giờ mới được phép unload Main Menu
        yield return SceneManager.UnloadSceneAsync("Main Menu Scene");

        Debug.Log("✅ Main Menu đã được unload khỏi bộ nhớ");
    }
    IEnumerator PreloadNextScene(string nextScene)
    {
        //Wait for first frame
        yield return null;

        preloadSceneOp = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        preloadSceneOp.allowSceneActivation = false;

        // Theo dõi tiến trình nếu muốn
        while (preloadSceneOp.progress < 0.9f)
        {
            Debug.Log("Loading progress: " + (preloadSceneOp.progress * 100) + "%");
            yield return null;
        }

        Debug.Log("Scene đã load xong và sẵn sàng để kích hoạt!");
    }


    public void OnEnable()
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
