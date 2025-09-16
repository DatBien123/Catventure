using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour
{
    public PlayableDirector director;   // Gán PlayableDirector vào đây
    public GameObject targetObject;     // GameObject cần active/disable
    
    public ParticleSystem ParticleSystem;

    public Material SkyboxSpaceMaterial;
    public Material SkyboxMapMaterial;

    [Header("Transform Origin")]
    public Vector3 ZeraOriginPosition;
    public Vector3 ZeraOriginRotation;
    public Vector3 ZeraOriginScale;

    [Header("References")]
    public CharacterPlayer Zera;

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

        Zera.transform.position = ZeraOriginPosition;
        Zera.transform.rotation = Quaternion.Euler(ZeraOriginRotation);
        Zera.transform.localScale = ZeraOriginScale;
    }

    void OnPlayVFX()
    {
        ParticleSystem.gameObject.SetActive(true);
        ParticleSystem.Play();
    }

    void OnAssignMapMaterial()
    {
        RenderSettings.skybox = SkyboxMapMaterial;
    }

    void OnAssignSpaceMaterial()
    {
        RenderSettings.skybox = SkyboxSpaceMaterial;
    }
}
