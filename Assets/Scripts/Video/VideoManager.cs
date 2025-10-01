using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] public GameObject videoPanel; // UI Panel chứa video
    private System.Action onVideoFinished;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    public void PlayVideo(VideoClip clip, System.Action callback)
    {
        onVideoFinished = callback;
        videoPanel.SetActive(true);

        videoPlayer.Stop();
        videoPlayer.clip = clip;

        // Ẩn panel tạm thời để tránh flash đen
        videoPanel.SetActive(false);

        // Đăng ký sự kiện khi chuẩn bị xong video
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        // Bỏ đăng ký để tránh lặp
        videoPlayer.prepareCompleted -= OnVideoPrepared;

        // Khi video đã sẵn sàng, bật panel và play
        videoPanel.SetActive(true);
        videoPlayer.Play();
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        onVideoFinished?.Invoke();
    }


}
