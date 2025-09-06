using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour {


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;


    private void Awake() {
        soundVolumeButton.onClick.AddListener(() => {
            LanderSoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "SOUND " + LanderSoundManager.Instance.GetSoundVolume();
        });
        musicVolumeButton.onClick.AddListener(() => {
            LanderMusicManager.Instance.ChangeMusicVolume();
            musicVolumeTextMesh.text = "MUSIC " + LanderMusicManager.Instance.GetMusicVolume();
        });
        resumeButton.onClick.AddListener(() => {
            LanderGameManager.Instance.UnpauseGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderMainMenuScene);
        });
    }

    private void Start() {
        LanderGameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        LanderGameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        soundVolumeTextMesh.text = "SOUND " + LanderSoundManager.Instance.GetSoundVolume();
        musicVolumeTextMesh.text = "MUSIC " + LanderMusicManager.Instance.GetMusicVolume();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);

        resumeButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}