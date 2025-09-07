using UnityEngine;
using UnityEngine.SceneManagement;

public static class LanderSceneLoader {

    public enum Scene {
        MainMenuScene,
        LanderGameScene,
        LanderGameOverScene,
    }

    public static void LoadScene(Scene scene) {
        SceneManager.LoadScene(scene.ToString());
    }

}