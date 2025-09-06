using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {



    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;


    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderMainMenuScene);
        });
    }

    private void Start() {
        scoreTextMesh.text = "FINAL SCORE: " + LanderGameManager.Instance.GetTotalScore().ToString();

        mainMenuButton.Select();
    }

}