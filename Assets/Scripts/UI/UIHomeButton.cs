using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHomeButton : MonoBehaviour {
    [SerializeField] private Button homeButton;

    private void OnEnable() {
        homeButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDisable() {
        homeButton.onClick.RemoveListener(BackToMainMenu);
    }

    private void BackToMainMenu() {
        SceneManager.LoadScene(Constants.HOME_SCENE_NAME);
    }
}
