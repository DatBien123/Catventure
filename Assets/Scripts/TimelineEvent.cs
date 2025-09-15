using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour
{
    public PlayableDirector director;   // Gán PlayableDirector vào đây
    public GameObject targetObject;     // GameObject cần active/disable
    
    public ParticleSystem ParticleSystem;

    void OnEnable()
    {
        // Đăng ký event
        director.played += OnTimelinePlay;
        director.stopped += OnTimelineStop;
    }

    void OnDisable()
    {
        // Hủy đăng ký event (tránh memory leak)
        director.played -= OnTimelinePlay;
        director.stopped -= OnTimelineStop;
    }

    void OnTimelinePlay(PlayableDirector obj)
    {
        // Khi timeline bắt đầu play
        targetObject.SetActive(false);
        //gameObject.transform.position = CutSceneTransform.position;
        //gameObject.transform.rotation = CutSceneTransform.rotation;
        //gameObject.transform.localScale = CutSceneTransform.localScale;
    }

    void OnTimelineStop(PlayableDirector obj)
    {
        // Khi timeline kết thúc (hoặc Stop)
        targetObject.SetActive(true);

        //gameObject.transform.position = OriginTransform.position;
        //gameObject.transform.rotation = OriginTransform.rotation;
        //gameObject.transform.localScale = OriginTransform.localScale;
    }

    void OnPlayVFX()
    {
        ParticleSystem.gameObject.SetActive(true);
        ParticleSystem.Play();
    }
}
