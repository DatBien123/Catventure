using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineEvent : MonoBehaviour
{
    #region [Old]
    //public PlayableDirector director;   // Gán PlayableDirector vào đây
    //public GameObject UIs;     // GameObject cần active/disable
    //public GameObject HomeCanvas;

    //public Material SkyboxSpaceMaterial;
    //public Material SkyboxMapMaterial;

    //[Header("Transform Origin")]
    //public Vector3 ZeraOriginPosition;
    //public Vector3 ZeraOriginRotation;
    //public Vector3 ZeraOriginScale;

    //[Header("References")]
    //public Dialogue Dialogue;
    //public CharacterPlayer Zera;

    //private void Start()
    //{
    //    if (Zera.isFirstTimeLogin)
    //    {
    //        director.Play();
    //    }
    //    else
    //    {
    //        UIs.SetActive(true);
    //        HomeCanvas.SetActive(true);
    //    }

    //}
    //void OnEnable()
    //{
    //    // Đăng ký event
    //    director.played += OnTimelinePlay;
    //    director.stopped += OnTimelineStop;
    //}

    //void OnDisable()
    //{
    //    // Hủy đăng ký event (tránh memory leak)
    //    director.played -= OnTimelinePlay;
    //    director.stopped -= OnTimelineStop;
    //}

    //void OnTimelinePlay(PlayableDirector obj)
    //{
    //    // Khi timeline bắt đầu play
    //    UIs.SetActive(false);
    //}

    //void OnTimelineStop(PlayableDirector obj)
    //{
    //    // Khi timeline kết thúc (hoặc Stop)
    //    UIs.SetActive(true);

    //    Zera.transform.position = ZeraOriginPosition;
    //    Zera.transform.rotation = Quaternion.Euler(ZeraOriginRotation);
    //    Zera.transform.localScale = ZeraOriginScale;

    //    //Dialogue
    //    DialogueData dialogueData = Dialogue.DialogueDataBase.Find(dialogueData => dialogueData.Topic == "First Time Open Game");
    //    Dialogue.UIDialogue.SetCurrentDialogueData(dialogueData);


    //    //Data
    //    Zera.isFirstTimeLogin = false;
    //    SaveSystem.Save(Zera, Zera.Inventory);
    //}

    //void OnAssignMapMaterial()
    //{
    //    RenderSettings.skybox = SkyboxMapMaterial;
    //}

    //void OnAssignSpaceMaterial()
    //{
    //    RenderSettings.skybox = SkyboxSpaceMaterial;
    //}
    #endregion
    #region [New]
    public PlayableDirector director;   // Gán PlayableDirector vào đây
    public GameObject UIs;     // GameObject cần active/disable
    public GameObject HomeCanvas;

    [Header("References")]
    public Dialogue Dialogue;
    public CharacterPlayer Zera;

    [Header("Skip button")]
    public Button Skip_Button;

    private void Start()
    {
        if (Zera.isFirstTimeLogin)
        {
            director.Play();
        }
        else
        {
            UIs.SetActive(true);
            HomeCanvas.SetActive(true);
        }

        Skip_Button.onClick.AddListener(() =>
        {
            director.Stop();
        });
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
    }

    void OnTimelineStop(PlayableDirector obj)
    {
        // Khi timeline kết thúc (hoặc Stop)
        UIs.SetActive(true);

        ////Dialogue
        DialogueData dialogueData = Dialogue.DialogueDataBase.Find(dialogueData => dialogueData.Topic == "First Time Open Game");
        Dialogue.UIDialogue.SetCurrentDialogueData(dialogueData);


        //Data
        Zera.isFirstTimeLogin = false;
        SaveSystem.Save(Zera, Zera.Inventory);
    }
    #endregion
}
