using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class StoryVideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public void PlayVideo(VideoClip clip, UnityAction onFinish)
    {
        if (clip != null)
        {
            videoPlayer.targetCameraAlpha = 1f;
            videoPlayer.clip = clip;
            videoPlayer.Play();
            videoPlayer.loopPointReached += _ => onFinish?.Invoke();
        }
        else { Debug.Log("Clip không tồn tại"); }
    }
    public void HideVideo()
    {
        videoPlayer.targetCameraAlpha = 0f; // Làm video biến mất sau khi phát xong
    }
}
