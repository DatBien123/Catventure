using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {



    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;


    private void Awake() {
        replayButton.onClick.AddListener(() => {
            LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderGameScene);
        });
        mainMenuButton.onClick.AddListener(() => {
            LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        scoreTextMesh.text = "FINAL SCORE: " + LanderGameManager.Instance.GetTotalScore().ToString();

        mainMenuButton.Select();
    }

}