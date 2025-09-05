using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {


    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;



    private void Awake() {
        Time.timeScale = 1f;

        playButton.onClick.AddListener(() => {
            LanderGameManager.ResetStaticData();
            LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderGameScene);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        playButton.Select();
    }

}