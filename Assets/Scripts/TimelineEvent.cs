using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour
{
    public PlayableDirector director;   // Gán PlayableDirector vào đây
    public GameObject UIs;     // GameObject cần active/disable
   

    public Material SkyboxSpaceMaterial;
    public Material SkyboxMapMaterial;

    [Header("Transform Origin")]
    public Vector3 ZeraOriginPosition;
    public Vector3 ZeraOriginRotation;
    public Vector3 ZeraOriginScale;

    [Header("References")]
    public CharacterPlayer Zera;

    private void Start()
    {
        if (Zera.isFirstTimeLogin)
        {
            director.Play();
        }
        else
        {
            UIs.SetActive(true);
        }

    }
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
        UIs.SetActive(false);

        //gameObject.transform.position = CutSceneTransform.position;
        //gameObject.transform.rotation = CutSceneTransform.rotation;
        //gameObject.transform.localScale = CutSceneTransform.localScale;
    }

    void OnTimelineStop(PlayableDirector obj)
    {
        // Khi timeline kết thúc (hoặc Stop)
        UIs.SetActive(true);

        Zera.transform.position = ZeraOriginPosition;
        Zera.transform.rotation = Quaternion.Euler(ZeraOriginRotation);
        Zera.transform.localScale = ZeraOriginScale;

        Zera.isFirstTimeLogin = false;
        SaveSystem.Save(Zera, Zera.Inventory);
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
