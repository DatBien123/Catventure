using UnityEngine;
using UnityEngine.SceneManagement;

public static class LanderSceneLoader {

    public enum Scene {
        LanderMainMenuScene,
        LanderGameScene,
        LanderGameOverScene,
    }


    public static void LoadScene(Scene scene) {
        SceneManager.LoadScene(scene.ToString());
    }

}